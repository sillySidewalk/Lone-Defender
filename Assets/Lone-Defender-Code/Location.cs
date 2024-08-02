using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

/*
    Any type of location where pawns can stand: Forest and Clearing
 */
public class Location : MonoBehaviour
{
    [SerializeField] protected int id;
    [SerializeField] protected Manager man;
    [SerializeField] protected int max_buildings;
    [SerializeField] protected List<Transform> positions; // List of transforms for position references
    //[SerializeField] public Transform enemy_position; // The position in the location where enemies are put
    [SerializeField] public TextMeshProUGUI enemy_count_txt; // A counter to show how many enemies are in the enemy_position
    //[SerializeField] public Transform player_position; // The position in the location where the player is put
    [SerializeField] public List<Building> buildings = new List<Building>(); // Number of buildings is limited
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
     * When a pawn moves to a clearing, Location decides where to put the pawn based on what move_position they have
     */
    public void add_pawn(Pawn p)
    {
        p.transform.position = positions[(int)p.move_pos].position;
    }


    protected void OnMouseDown()
    {
        man.location_click(this);
    }


}
