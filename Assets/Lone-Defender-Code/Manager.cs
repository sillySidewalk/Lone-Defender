using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Xml;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public List<Clearing> clearings;
    public List<Road> roads;
    public List<Forest> forests;
    [SerializeField] protected GameObject game_state_obj;
    [SerializeField] protected GameObject sub_state_obj;
    [SerializeField] public GameObject events_obj;
    public SerializedDictionary<string, game_state> game_states = new();
    public SerializedDictionary<string, sub_state> sub_states = new();
    public game_state current_game_state;
    public sub_state current_sub_state;
    protected List<sub_state> loc_clk_sstage_subscription = new List<sub_state>(); // which substates want to be told about a Location click
    public Player player;
    public Random_manager ran_man;
    public Enemy_manager enemy_man;
    public quest_system qs;
    [SerializeField] protected string next_state = null;
    [SerializeField] protected string next_sub_state = null;
    // public Enemy_manager e_man; // Can be access from game_states
    //public int dice_value = 10; // The type of dice
    public int min_atk_val { get; } = 8; // What value is considered a hit, base d10 dice
    protected int item_id = 0; // The id handed out to other objects that request it
    [SerializeField] protected TextMeshProUGUI action_point_ui;
    [SerializeField] public GameObject enemy_holder; // All enemy pawns will be put under this object for easy of visuals


    //[SerializeField] public dh_gameobject dh_prefabs;
    [SerializeField] public SerializedDictionary<String, GameObject> prefabs;

    [SerializeField] public float enemy_march_anim_time = .5f; // How long, in seconds, to wait between enemy march animation

    //[SerializeField] protected List<IDisplay_location_helper> display_setup_list; // A list of all classes that need a display location


    private void Awake()
    {
        init();
    }


    public void init()
    {
        init_clearings();
        init_enemy_man();
        init_game_states();
        init_sub_states();
        init_player();
        init_quest_system();

        request_change_state("player_turn");
    }

    // Check if we need to update the current state after something happens
    protected void check_state()
    {
        if (next_state != null)
        {
            string temp_next_state = next_state;
            next_state = null;
            change_game_state(temp_next_state);
        }
        else if (next_sub_state != null)
        {
            string temp_next_sub_state = next_sub_state;
            next_sub_state = null;
            change_sub_state(temp_next_sub_state);
        }
        
    }


    /*
     * An attack on enemies to Clearing
     * parameters:
     *      List of ints representing attacks
     *      Clearing they are attacking
     */
    public void attack_enemy(List<int> attacks, Clearing cl)
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

        deal_hits_to_enemy(hits, cl);
    }

    /*
     * Hit enemies and remove them
     */
    public void deal_hits_to_enemy(int hits, Clearing cl)
    {
        List<Enemy> enemies = cl.get_enemies();
        Pawn p;

        if(enemies != null && enemies.Count > 0)
        {
            // For each hit, remove an enemy pawn, stop if we run out of hits or run out of Enemy Pawns
            for (; hits > 0 && enemies.Count > 0; hits--)
            {
                p = enemies[0];
                cl.remove_pawn(p);
                enemies.RemoveAt(0);
                Destroy(p.gameObject);
            }
        }

        if(hits <= 0)
        {
            return;
        }


        // Since there should only be one building per location, just grab first
        List<Building> buildings = cl.get_buildings();
        if(buildings.Count > 0)
        {
            Building b = cl.get_buildings()[0];

            b.apply_hits(hits);
        }
        
    }

    public bool is_player_at(Location loc)
    {
        if(player.current_location == loc)
        {
            return true;
        }
        else
        {
            return false;
        }
            
    }

    // Check if player is at clearing. If so, deliver attacks
    public void attack_player(List<int> atks, Clearing cl)
    {
        if(!is_player_at(cl))
        {
            return;
        }

        player.receive_atk(atks);
    }

    public void attack_player(List<int> atks)
    {
        player.receive_atk(atks);
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
            sstate.init();
            
            // TODO: move this to the init of each sub_state
            // If they want a location click event
            if(sstate.loc_click_sub)
            {
                loc_clk_sstage_subscription.Add(sstate);
            }
        }

    }

    protected void init_game_states()
    {
        List<game_state> gstates = game_state_obj.GetComponents<game_state>().ToList();


        foreach(game_state gstate in gstates)
        {
            game_states.Add(gstate.game_state_name, gstate);
            gstate.init();
        }

    }

    

    protected void init_clearings()
    {
        foreach(Clearing c in clearings)
        {
            c.init();
        }
    }

    protected void init_player()
    {
        player.init(request_id(), clearings[0], action_point_ui);
    }

    protected void init_enemy_man()
    {
        enemy_man.init();
    }

    protected void init_quest_system()
    {
        qs.init();
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
     * Pass to substate, mostly for changing state
     */
    public void call_sub_state(String sub_state_name)
    {
        current_sub_state.call(sub_state_name);
    }

    public void request_change_state(string game_state_name)
    {
        next_state = game_state_name;
    }

    public void request_change_sub_state(string sub_state_name)
    {
        next_sub_state = sub_state_name;
    }

    /*
     * End the previous sub_state and start the new one
     */
    protected void change_sub_state(String sub_state_name)
    {
        if (current_sub_state != null)
        {
            current_sub_state.end_state();
        }

        if(!sub_states.ContainsKey(sub_state_name)) // prevent infinite loop
        {
            Debug.LogError("sub_state_name: " + sub_state_name + " is not in sub_states");
            next_sub_state = null;
            return;
        }

        current_sub_state = sub_states[sub_state_name];
        current_sub_state.start_state();
    }

    protected void change_game_state(String game_state_name)
    {
        if(current_game_state != null)
        {
            current_game_state.end_state();
        }

        if (!game_states.ContainsKey(game_state_name)) // prevent infinite loop
        {
            Debug.LogError("game_state_name: " + game_state_name + " is not in game_states_states");
            next_state = null;
            return;
        }

        current_game_state = game_states[game_state_name];
        current_game_state.start_state();
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

        //debug_print_distance_list(distance_list);

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

    public int request_id()
    {
        int ret_val = item_id;
        item_id++;
        return ret_val;
    }

    // For debugging, adding corruption_tokens to a location
    public void add_corruption_to_location(Location loc, int num_tokens)
    {
        for(int i = 0; i < num_tokens; i++)
        {
            GameObject t_go = Instantiate(prefabs["corruption_token"]);
            LD_token new_t = t_go.GetComponent<corruption_token>();

            loc.add_token(new_t, new_t.GetType().Name);
        }
        
    }

    public void call_quest()
    {
        List<int> dice_attemps = player.quest_attempts();

        qs.attempt_quest_clr( (Clearing) player.current_location, dice_attemps);
    }



    /* For debugging
     * 
     * Since enemies need factory for certain displays, either pass in a factory or null, then create minimal factory
     */
    public void spawn_enemies(int amount, int clearing_num, factory fact)
    {
        if(fact == null)
        {
            fact = GameObject.Instantiate(prefabs["factory"]).GetComponent<factory>();
            fact.init(0, this, enemy_man);
            fact.loc = clearings[0];
        }

        Clearing c = clearings[clearing_num];
        for (int i = 0; i < amount; i++)
        {
            GameObject e_obj = Instantiate(prefabs["Enemy"]);
            Enemy e = e_obj.GetComponent<Enemy>();

            e.init(request_id(), c, this, enemy_man, fact);
        }
    }


    // While testing enemy_spawn, I need a several enemy_spawn and nothing else
    protected void debug_all_enemy_spawns()
    {
        enemy_man.debug_bag_fill("enemy_spawn", 30);
    }

    /*
     * testing the retaliation and stealth system
     * 
     * Spawn enemies, have player attack
     * 
     * A new thing I'm trying, instead of creating tests and then removing them, I'll create a function that contains all the parts of the test, with each part having a number. Then just make key presses call the relevant parts. This way tests can stay around and hopefully be more clear later
     */
    public void test_retal_stealth(int part)
    {
        // spawn enemies at clearing 0, where player starts
        if(part == 0)
        {
            spawn_enemies(9, 0, null);
        }
    }


    /*
     * Testing enemy attack at end of turn
     */
    public void test_en_atk(int part)
    {
        if(part == 0)
        {
            spawn_enemies(9, 0, null);
        }
        if(part == 1)
        {
            request_change_sub_state("enemy_attack");
        }
    }

    /*
     * Test auto_exit_sub_state and get_next
     */
    public void test_auto_ex_and_get_next(int part)
    {
        // start Enemy_turn_state, which should automatically start it's action order. Should add debut logging at start of states for easy verification
        if(part == 0)
        {
            request_change_state("Enemy_turn_state");
        }
    }


    private void Update()
    {
        check_state();

        if (Input.GetKeyDown("d"))
        {
            enemy_man.debug_bag_fill("enemy_scoring", 10);
        }

        if (Input.GetKeyDown("e"))
        {
            change_sub_state("enemy_event");
        }

        if (Input.GetKeyDown("r"))
        {
            //change_game_state("Enemy_manager_state");
            //change_sub_state("enemy_spawn");

            enemy_man.create_enemies();
            
        }

        if( Input.GetKeyDown("s"))
        {
            Enemy_turn_state em = (Enemy_turn_state)game_states["Enemy_manager_state"];
            //em.enemies[0].march();
        }

        if(Input.GetKeyDown("t"))
        {
            clearings[0].print_pawns();
        }

        if(Input.GetKeyDown("q"))
        {
            enemy_man.move_spawn();
        }

        /* testing location_by_distance
        if(Input.GetKeyDown("1"))
        {
            remove_all_highlights();
            List<Location> locs = location_by_distance(forests[5], 0, 2); 
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
