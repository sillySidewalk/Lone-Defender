using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

/*
    Any type of location where pawns can stand: Forest and Clearing
 */
public abstract class Location : MonoBehaviour
{
    [SerializeField] protected int id;
    [SerializeField] protected Manager man;
    [SerializeField] public int max_buildings;
    [SerializeField] protected List<Transform> positions; // List of transforms for position references
    //[SerializeField] public Transform enemy_position; // The position in the location where enemies are put
    [SerializeField] public TextMeshProUGUI enemy_count_txt; // A counter to show how many enemies are in the enemy_position
    //[SerializeField] public Transform player_position; // The position in the location where the player is put
    [SerializeField] public List<Building> buildings = new List<Building>(); // Number of buildings is limited
    [SerializeField] protected List<Transform> building_locs = new List<Transform>(); // Where the buildings will go
    [SerializeField] public List<LD_Token> tokens = new List<LD_Token>(); // Number of tokens is not limited
    [SerializeField] public List<Pawn> enemy_pawns = new List<Pawn>(); // Pawns are not limited (Probably)
    [SerializeField] public List<Pawn> player_pawns = new List<Pawn>(); // Pawns are not limited (Probably)
    [SerializeField] public List<Location> adjacent_locations = new List<Location>(); // Clearings and Forests
    [SerializeField] public List<Road> adjacent_roads = new List<Road>(); // slightly different context between forest and clearing, but I think it'll be ok
    public GameObject location_highlighter;

    public enum move_position // Which of the move positions to be in, like player or enemy
    {
        enemy,
        player,
    }

    public int get_id()
    {
        return id;
    }

    /*
     * When a pawn moves to a clearing, Location decides where and what array to put the pawn based on what move_position they have
     * 
     * if a location has specific stuff, override, then call base.add_pawn(p)
     */
    public virtual void add_pawn(Pawn p)
    {
        if (p.move_pos == move_position.enemy)
        {
            enemy_pawns.Add(p);
        }
        else if (p.move_pos == move_position.player)
        {
            player_pawns.Add(p);
        }

        p.transform.position = positions[(int)p.move_pos].position;
    }

    public virtual void remove_pawn(Pawn p)
    {
        if(p.move_pos == move_position.enemy)
        {
            enemy_pawns.Remove(p);
        }
        else if(p.move_pos == move_position.player)
        {
            player_pawns.Remove(p);
        }
    }

    /*
     * add a building to this location. If there are already the max buildings, return false, else true
     */
    public bool add_building(Building b)
    {
        if(buildings.Count >= max_buildings)
        {
            // no room for building
            return false;
        }

        buildings.Add(b);
        b.loc = this;

        // give the building prefab the correct building position, minus 1 to account for 0 index array
        b.transform.position = building_locs[buildings.Count - 1].position;

        return true;
    }


    /*
     * Remove the building, return false if it's not in this location
     */
    public void remove_building(Building b)
    {
        buildings.Remove(b);
    }


    protected void OnMouseDown()
    {
        man.location_click(this);
    }


}
