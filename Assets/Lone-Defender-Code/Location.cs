using System;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using AYellowpaper.SerializedCollections;
using JetBrains.Annotations;
using Unity.VisualScripting;

/*
    Any type of location where pawns can stand: Forest and Clearing
 */
public abstract class Location : MonoBehaviour
{
    [SerializeField] protected int id;
    abstract public string location_type { get; }
    [SerializeField] protected Manager man;
    [SerializeField] public int max_buildings;
    [SerializeField] protected List<Transform> display_positions = new(); // List of transforms for position references
    //[SerializeField] public Transform enemy_position; // The position in the location where enemies are put
    //[SerializeField] public TextMeshProUGUI enemy_count_txt; // A counter to show how many enemies are in the enemy_position
    //[SerializeField] public Transform player_position; // The position in the location where the player is put
    [SerializeField] protected List<Building> buildings = new (); // Number of buildings is limited
    [SerializeField] protected List<Transform> building_locs = new List<Transform>(); // Where the buildings will go
    [SerializeField] public Dictionary<string, List<LD_token>> tokens = new (); // string is token type name, Number of tokens is not limited
    //[SerializeField] protected dh_pawns dh_pawns;
    [SerializeField] public SerializedDictionary<String, List<Pawn>> pawns = new();
    //[SerializeField] public List<Enemy> enemy_pawns = new (); // Pawns are not limited (Probably)
    //[SerializeField] public List<Pawn> player_pawns = new List<Pawn>(); // Pawns are not limited (Probably)
    [SerializeField] public List<Location> adjacent_locations = new List<Location>(); // Clearings and Forests
    [SerializeField] public List<Road> adjacent_roads = new List<Road>(); // slightly different context between forest and clearing, but I think it'll be ok
    public GameObject location_highlighter;

    [SerializeField] protected List<GameObject> display_slot = new List<GameObject>(); // the Position that each location display will go, currently has pre-locations
    //[SerializeField] protected dict_helper_loc_display dh_display;
    [SerializeField] protected SerializedDictionary<String, Location_display> display_dict; // for use when adding game piece, game pieces will have display name to reference which to add them
    //[SerializeField] protected List<string> display_dict_keys = new();
    //[SerializeField] protected List<Location_display> display_dict_values = new();



    public enum move_position // Which of the move positions to be in, like player or enemy
    {
        enemy,
        player,
    }

    public int get_id()
    {
        return id;
    }

    public virtual void init()
    {
        init_displays();
    }

    /* Replaced by dict_helper
    public virtual void init_display_dict()
    {
        if(display_dict_keys.Count != display_dict_values.Count)
        {
            Debug.LogError("display_dict_keys.Count must equal display_dict_values.Count");
            return;
        }

        for (int i = 0; i < display_dict_keys.Count; i++)
        {
            display_dict.Add(display_dict_keys[i], display_dict_values[i]);
        }
    }
    */

    protected void init_displays()
    {
        
        foreach(KeyValuePair<string, Location_display> entry in display_dict)
        {
            entry.Value.init(this);
        }
    }

    /*
     * Give the GameObjects for the position requested and set the display in the given slot
     */
    public List<GameObject> request_pos(int positions, Location_display display)
    {
        return null;
    }

    public List<Building> get_buildings()
    {
        return buildings;
    }

    /*
     * Check the various dictionary of game pieces and return the count of that type
     */
    public int get_count(string type)
    {
        if(tokens.ContainsKey(type))
        {
            return tokens[type].Count;
        }    
        else if(pawns.ContainsKey(type))
        {
            return pawns[type].Count;
        }
        else
        {
            Debug.LogError("type wasn't found");
            return 0;
        }
    }

    public virtual void add_to_display(Game_piece gp)
    {
        display_dict[gp.GetType().Name].add_game_piece(gp);
    }

    public virtual void remove_from_display(Game_piece gp)
    {
        display_dict[gp.GetType().Name].remove_game_piece(gp);
    }

    public virtual void add_pawn(Pawn p)
    {
        string p_name = p.GetType().Name;
        if (!pawns.ContainsKey(p_name))
        {
            pawns.Add(p_name, new List<Pawn>());
        }
        pawns[p_name].Add(p);
        add_to_display((Game_piece)p);
    }

    /* Old Version
     * 
     * 
     * 
     * When a pawn moves to a clearing, Location decides where and what array to put the pawn based on what move_position they have
     * 
     * if a location has specific stuff, override, then call base.add_pawn(p)
     */
    /*
    public virtual void add_pawn(Pawn p)
    {
        if (p.move_pos == move_position.enemy)
        {
            enemy_pawns.Add((Enemy)p);
        }
        else if (p.move_pos == move_position.player)
        {
            player_pawns.Add(p);
        }

        p.transform.position = display_positions[(int)p.move_pos].position;
    }
    */

    public virtual void remove_pawn(Pawn p)
    {        
        pawns[p.GetType().Name].Remove(p);

        remove_from_display((Game_piece)p);
    }


    /* Old Version
    public virtual void remove_pawn(Pawn p)
    {
        if(p.move_pos == move_position.enemy)
        {
            enemy_pawns.Remove((Enemy)p);
        }
        else if(p.move_pos == move_position.player)
        {
            player_pawns.Remove(p);
        }
    }
    */

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


    public void add_token(LD_token t)
    {

    }


    protected void OnMouseDown()
    {
        man.location_click(this);
    }


}
