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
    [SerializeField] public SerializedDictionary<string, List<LD_token>> tokens = new (); // string is token type name, Number of tokens is not limited
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
            //Debug.LogError("type wasn't found");
            return 0;
        }
    }

    public List<Enemy> get_enemies()
    {
        if (pawns.ContainsKey("Enemy"))
        {
            return pawns["Enemy"].ConvertAll(x => (Enemy)x);
        }
        else
        {
            return null;
        }

        
    }


    // Assume display is gamepiece name, other add_to_display if it needs to be specified
    public virtual void add_to_display(Game_piece gp)
    {
        display_dict[gp.GetType().Name].add_game_piece(gp);
    }

    // Because some tokens share a space (e.g. events), it should be easier to accept a string for the display they want
    public virtual void add_to_display(Game_piece gp, string display_type)
    {
        display_dict[display_type].add_game_piece(gp);
    }

    /*
    public virtual void add_to_displau(Game_piece gp, String gp_type)
    {
        display_dict[gp_type].add_game_piece(gp);
    }
    */

    public virtual void remove_from_display(Game_piece gp, string display_type)
    {
        display_dict[display_type].remove_game_piece(gp);
    }

    public virtual void add_pawn(Pawn p)
    {
        string p_name = p.GetType().Name;
        if (!pawns.ContainsKey(p_name))
        {
            pawns.Add(p_name, new List<Pawn>());
        }
        pawns[p_name].Add(p);
        add_to_display((Game_piece)p, p.GetType().Name);
    }

    public virtual void remove_pawn(Pawn p)
    {        
        pawns[p.GetType().Name].Remove(p);

        remove_from_display((Game_piece)p, p.GetType().Name);
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

        add_to_display((Game_piece)b, b.GetType().Name);

        return true;
    }


    /*
     * Remove the building, return false if it's not in this location
     */
    public void remove_building(Building b)
    {
        buildings.Remove(b);
        remove_from_display(b, b.GetType().Name);
    }

    public void add_token(LD_token t)
    {
        string t_name = t.GetType().Name;
        if (!tokens.ContainsKey(t_name))
        {
            tokens.Add(t_name, new List<LD_token>());
        }
        tokens[t_name].Add(t);
        add_to_display((Game_piece)t);
        t.loc = this;
    }


    public void add_token(LD_token t, string display_type)
    {
        string t_name = t.GetType().Name;
        if (!tokens.ContainsKey(t_name))
        {
            tokens.Add(t_name, new List<LD_token>());
        }
        tokens[t_name].Add(t);
        add_to_display((Game_piece)t, display_type);
        t.loc = this;
    }


    public void remove_token(LD_token t)
    {
        tokens[t.GetType().Name].Remove(t);

        remove_from_display((Game_piece)t, t.GetType().Name);

        t.remove();
    }

    // For when the display type needs to be specified
    public void remove_token(LD_token t, string display_type)
    {
        tokens[t.GetType().Name].Remove(t);

        remove_from_display((Game_piece)t, display_type);

        t.remove();
    }


    protected void OnMouseDown()
    {
        man.location_click(this);
    }


}
