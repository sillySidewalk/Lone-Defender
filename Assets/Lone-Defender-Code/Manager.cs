using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Xml;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public List<Clearing> clearings;
    public List<Road> roads;
    public List<Forest> forests;
    [SerializeField] protected GameObject game_state_obj;
    [SerializeField] protected GameObject sub_state_obj;
    public Dictionary<string, game_state> game_states = new();
    public Dictionary<string, sub_state> sub_states = new();
    public sub_state current_sub_state;
    public game_state current_game_state;
    protected List<sub_state> loc_clk_sstage_subscription = new List<sub_state>(); // which substates want to be told about a Location click
    public Player player;
    System.Random rnd = new System.Random();
    int dice_value = 10; // The type of dice
    int min_atk_val { get; } = 8; // What value is considered a hit, base d10 dice

    public enum Gamestate
    {
        player_turn,
    }

    public enum substate
    {
        player_defaut,
        player_move,
    }

    private void Awake()
    {
        init();
    }


    public void init()
    {
        init_sub_states();
        init_game_states();
    }


    /*
     * Roll the dice n times
     */
    public List<int> d_roll(int n)
    {
        List<int> return_dice = new List<int>();

        for(int i = 0; i < n; i++)
        {
            return_dice.Add(rnd.Next(1, dice_value + 1)); // Max is exclusive, so plus 1
        }
        
        return return_dice;

    }


    /*
     * An attack (from player pawn) to Clearing
     * parameters:
     *      List of ints representing attacks
     *      Clearing they are attacking
     */
    public void player_pawn_attack(List<int> attacks, Clearing cl)
    {
        int def_mod = cl.get_en_def_sum();
        int hits = 0;

        foreach(int atk in attacks)
        {
            if ((atk - def_mod) >= min_atk_val )
            {
                hits++;
            }
        }

        deal_player_hits(hits, cl);
    }


    public void deal_player_hits(int hits, Clearing cl)
    {
        List<Pawn> e = cl.enemy_pawns;
        Pawn p;

        if(e.Count > 0)
        {
            // For each hit, remove an enemy pawn, stop if we run out of hits or run out of Enemy Pawns
            for (; hits > 0 && e.Count > 0; hits--)
            {
                p = e[0];
                e.RemoveAt(0);
                Destroy(p.gameObject);
            }
        }

        if(hits > 0)
        {
            Debug.Log("Remaining " + hits + " hits would go to Buildings or Tokens");
        }
        
    }

    
    /*
     * Move the enemy pawns. They can only move from one clearing to adjacent clearing     
     */
    public void move_enemies(Clearing start_cl, Clearing end_cl, int num)
    {
        List<Pawn> e_pawns = start_cl.enemy_pawns;

        // Check if there's enough enemy pawns
        if( e_pawns.Count < num )
        {
            Debug.LogError("Not enough enemies in start clearing");
            return;
        }

        // Since all enemies are the sane, just move the first num enemies
        List<Pawn> moving_pawns = e_pawns.Take(num).ToList();
        
        e_pawns.RemoveRange(0, num);

        foreach(Pawn p in moving_pawns)
        {
            p.move(end_cl);
        }

        end_cl.enemy_count_txt.text = num.ToString();
    }


    /*
     * For initializing, get the sub_state game object with all the sub_states, then add them to the list of sub_states
     */
    protected void init_sub_states()
    {
        List<sub_state> sstates = sub_state_obj.GetComponents<sub_state>().ToList();

        foreach(sub_state sstate in sstates)
        {
            sub_states.Add(sstate.sub_state_name, sstate);
            

            // If they want a location click event
            if(sstate.loc_click_sub)
            {
                loc_clk_sstage_subscription.Add(sstate);
            }
        }

        // set choose_sub_state as initial state and call it's start
        current_sub_state = sub_states["player_choose"];
        current_sub_state.start_state();
    }

    protected void init_game_states()
    {
        List<game_state> gstates = game_state_obj.GetComponents<game_state>().ToList();

        foreach(game_state gstate in gstates)
        {
            game_states.Add(gstate.game_state_name, gstate);
        }

        current_game_state = game_states["player_turn"];
        current_game_state.start_state();
    }


    /*
     * When a Location is clicked, it will call this. Then Manager will check if the current sub state wants to be notified, then send over the information
     */
    public void location_click(Location loc)
    {
        if(loc_clk_sstage_subscription.Contains(current_sub_state))
        {
            current_sub_state.loc_click(loc);
        }
    }


    /*
     * If not in the sub_state, change into the sub_state. If in the sub_state already, pass the message onto the substate
     */
    public void call_sub_state(String sub_state_name)
    {
        if (current_sub_state.sub_state_name != sub_state_name)
        {
            change_sub_state(sub_state_name);
        }
        else
        {
            current_sub_state.call();
        }

    }

    /*
     * End the previous sub_state and start the new one
     */
    public void change_sub_state(String sub_state_name)
    {
        current_sub_state.end_state();
        current_sub_state = sub_states[sub_state_name];
        current_sub_state.start_state();
    }

    /*
     * Given a start and end, find the shortest path. Since the graph is so small, I'll just use breadth first instea of A*
     */
    public List<Location> find_path(Location start_loc, Location end_loc, Pawn.move_type m_type)
    {
        List<Location> visited = new();
        Queue<Location> to_visit = new();
        Dictionary<Location, Location> from_loc = new(); // How a given Location was reached, the key is a given Location and the value was how that Location was reached

        to_visit.Enqueue(start_loc);
        visited.Add(start_loc);
        Location current_loc = null;
        List<Location> adj_loc = new();

        while(to_visit.Count > 0)
        {
            current_loc = to_visit.Dequeue();

            adj_loc = adjacent_by_type(current_loc, m_type);

            foreach(Location loc in adj_loc)
            {
                if(!visited.Contains(loc))
                {
                    visited.Add(loc);
                    to_visit.Enqueue(loc);
                    from_loc.Add(loc, current_loc);
                }
            }
        }

        // Backtrack through from_loc to find the path
        List<Location> path = new();
        path.Add(end_loc);
        Location cur_loc = end_loc;
        Location next_loc = null;

        while (cur_loc != start_loc)
        {
            next_loc = from_loc[cur_loc];
            path.Add(next_loc);
            cur_loc = next_loc;
        }

        path.Reverse(); // Flipping it so that start_loc is first

        return path;
    }

    /*
     * Get all the adjacent Locations by the type of movement
     */
    public List<Location> adjacent_by_type(Location loc, Pawn.move_type m_type)
    {
        if (m_type == Pawn.move_type.clearings)
        {
            return loc.adjacent_locations.OfType<Clearing>().Cast<Location>().ToList();
        }
        else if(m_type == Pawn.move_type.forests)
        {
            return loc.adjacent_locations.OfType<Forest>().Cast<Location>().ToList();
        }
        else // m_type == Pawn.move_type.clear_for
        {
            return loc.adjacent_locations;
        }
    }

    /*
     * For finding all the Locations that are some distance away. Using breadth first.
     * Returns Locations that are between start_distance and end_distance distance, inclusive
     * Right now all my use cases involve clearings, so it only counts clearings
     */
    public List<Location> location_by_distance(Location start_loc, int start_distance, int end_distance)
    {
        // the outer List is the distance, the inner list is the locations at that distance
        List<List<Location>> distance_list = new();
        List<Location> visited = new();
        Queue<(Location, int)> to_visit = new(); // int is for distance from start_loc

        to_visit.Enqueue((start_loc, 0));
        visited.Add(start_loc);
        Location current_loc = null;
        int current_dist = 0;
        List<Location> adj_loc = new();

        // setup the start of distance_list
        distance_list.Add(new List<Location>());
        distance_list[0].Add(start_loc);
        

        while (to_visit.Count > 0)
        {
            (Location, int) current_tuple = to_visit.Dequeue();
            current_loc = current_tuple.Item1;
            current_dist = current_tuple.Item2;

            adj_loc = adjacent_by_type(current_loc, Pawn.move_type.clearings);

            foreach (Location loc in adj_loc)
            {
                if (!visited.Contains(loc))
                {
                    // The distance of the currenct node
                    int this_dist = current_dist + 1;
                    visited.Add(loc);
                    to_visit.Enqueue((loc, this_dist));
                    
                    if(distance_list.Count - 1 < this_dist) // Check if we need to add the next layer of distance_list
                    {
                        distance_list.Add(new List<Location>());
                    }
                    distance_list[this_dist].Add(loc);
                }

                //Debug.Log("to_visit: " + to_visit.ToArray());
            }
        }

        debug_print_distance_list(distance_list);

        // Merge all the distances that was asked for
        List<Location> final_list = new();
        // This way, if someone asks for a greater distance than exists, they won't go out of bounds for distance_list. Because the distances relate to the index, not the count, we need Count - 1
        end_distance = Mathf.Clamp(end_distance, end_distance, distance_list.Count-1);
        for(int i = start_distance; i <= end_distance; i++)
        {
            final_list.AddRange(distance_list[i]);
        }

        return final_list;
    }

    // For debugging Location_by_distance, printing out the distance list
    public void debug_print_distance_list(List<List<Location>> dist_list)
    {
        int level = 0;
        foreach(List<Location> loc_list in dist_list)
        {
            List<int> ids = loc_list.Select(l => l.get_id()).ToList();

            Debug.Log("Level " + level + ": " + String.Join(",", ids));
            level++;
        }

    }

    /*
     * Activate the highlighter
     */
    public void hightlight_loc(List<Location> locs)
    {
        foreach (Location loc in locs)
        {

            loc.location_highlighter.SetActive(true);
        }
    }

    /*
     * Remove all of the highlights
     */
    public void remove_all_highlights()
    {
        foreach (Location loc in clearings)
        {
            loc.location_highlighter.SetActive(false);
        }

        foreach (Location loc in forests)
        {
            loc.location_highlighter.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown("d"))
        {
            Debug.Log("sub_states");
            foreach(KeyValuePair<string, sub_state> entry in sub_states)
            {
                Debug.Log(entry.Key + " = " + entry.Value);
            }

            Debug.Log("game_states");
            foreach (KeyValuePair<string, game_state> entry in game_states)
            {
                Debug.Log(entry.Key + " = " + entry.Value);
            }
        }

        /* testing location_by_distance
        if(Input.GetKeyDown("1"))
        {
            remove_all_highlights();
            List<Location> locs = Location_by_distance(clearings[6], 0, 2); 
            List<int> output = locs.Select(l => l.get_id()).ToList();

            Debug.Log(String.Join(",", output));
            hightlight_loc(locs);
        }
        //*/

        /* Testing path
        if(Input.GetKeyDown("2"))
        {
            remove_all_highlights();
            int start = rnd.Next(0, 12);
            int end = rnd.Next(0, 12);

            List<Location> path = find_path(clearings[start], clearings[end], Pawn.move_type.clearings);
            hightlight_loc(path);

            List<int> output = path.Select(l => l.get_id()).ToList();
            Debug.Log("start: " + start + " end: " +  end);
            Debug.Log(String.Join(",", output));
        }
        //*/
    }

}
