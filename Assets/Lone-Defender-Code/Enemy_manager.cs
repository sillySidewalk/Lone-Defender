using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEditor.Progress;



/*
 * Handles controlling the enemy
 */

public class Enemy_manager : MonoBehaviour
{
    public Manager man;
    public List<Enemy> enemies = new();
    public List<spawn> enemy_spawns; // Because there are only 2 types of buildings and a spawn is always attached to a factory, easier to just have a list of each
    public List<factory> enemy_factories;
    [SerializeField] protected int spawn_factory_amount = 1; // How many spawns and factories to make
    [SerializeField] protected List<int> spawn_starting_clearing = new List<int>() { 1 }; // Which clearing the given spawn should start in 
    [SerializeField] protected List<int> factory_starting_clearing = new List<int> { 10 };
    [SerializeField] protected List<sub_state> sub_states; // The possible turn types
    [SerializeField] protected List<string> sstate_bag_mandatory = new List<string>(); // what enemy actions must always happen each cycle (from enemy_scoring to enemy_scoring)
    [SerializeField] protected List<string> sstate_bag_optional = new List<string>(); // The bag other actions can occur
    [SerializeField] protected  List<string> sstate_order; // The order that turn types will occur
    [SerializeField] protected int actions_this_turn = 0;
    [SerializeField] protected int actions_per_turn = 2; // how many actions the enemy gets per player turn
    [SerializeField] protected int score; // how many victory points the enemy has, which leads to their victorys
    [SerializeField] public retaliation_system retal_system;
    [SerializeField] public int enemy_atk_dice = 2; // how many dice per enemy in attack or retaliation

    public int spawn_const_amount { get; } = 1; // When spawning, the amount of enemies is: spawn_const_amount + (spawn_dice_amount)d4
    public int spawn_dice_amount { get; } = 1;

    public void init()
    {
        init_buildings();
        init_substate_bag();
        fill_sstate_order();
    }

    protected void init_buildings()
    {
        for (int i = 0; i < spawn_factory_amount; i++)
        {
            GameObject s_go = Instantiate(man.prefabs["spawn"], new Vector3(0, 0, 0), Quaternion.identity);
            spawn s = s_go.GetComponent<spawn>();
            s.init(man.request_id(), man, this);
            enemy_spawns.Add(s);
            Clearing c = man.clearings[spawn_starting_clearing[i]];
            s.move(c);

            GameObject f_go = Instantiate(man.prefabs["factory"], new Vector3(0, 0, 0), Quaternion.identity);
            factory f = f_go.GetComponent<factory>();
            f.init(man.request_id(), man, this);
            enemy_factories.Add(f.GetComponent<factory>());
            c = man.clearings[factory_starting_clearing[i]];
            f.move(c);

            // add factory to spawn
            s.fact = f;
        }
    }

    protected void init_substate_bag()
    {
        // Since I will probably populate this in the inspector, this will check if bag is null
        if(sstate_bag_optional.Count == 0)
        {
            Debug.LogError("Enemy_manager sub_state bag is empty");
        }

    }

    /*
     * Fill sstate_order from sstate_bag, doesn't change sstate_bag
     */
    protected void fill_sstate_order()
    {
        List<string> temp_sstate_optional = man.ran_man.randomize_list<string>(new List<string>(sstate_bag_optional));
        List<string> temp_sstate_mandatory = new List<string>(sstate_bag_mandatory);
        

        // Add the scoring every 5 - 7 turns, because turns consist of 2 enemy actions and I want scoring to be the second action of the turn, it's (turn_number * 2) -1
        int scoring_turn = man.ran_man.random_num(5, 7);
        int actions_till_scoring = (scoring_turn * 2) - 1;

        // to keep enemy_spawns at a reasonable number, set equal to the number of turns till scoring, plus or minus 1
        int num_enemy_spawns = scoring_turn + (man.ran_man.random_num(0, 1) - 1);

        for(int i = 0; i < num_enemy_spawns; i++)
        {
            temp_sstate_mandatory.Add("enemy_spawn");
        }

        // how many optional actions to grab
        int num_optional_actions = actions_till_scoring - temp_sstate_mandatory.Count;

        temp_sstate_mandatory.AddRange(temp_sstate_optional.GetRange(0, num_optional_actions));

        // Add to current sstate_order, in case it's not empty
        sstate_order.AddRange(man.ran_man.randomize_list<string>(temp_sstate_mandatory));


        // TODO: add enemy_scoring to the end
        sstate_order.Add("enemy_scoring");
    }

    /*
     * Get the next state and remove it from sstate_order
     */
    public string get_next()
    {
        // if enemy has used their action for their turn, switch to end turn
        if(actions_this_turn >= actions_per_turn)
        {
            return "enemy_end_turn";
        }

        actions_this_turn++;

        // refill the bag
        if (sstate_order.Count == 0)
        {
            fill_sstate_order();
        }

        return sstate_order.Dequeue();
    }


    /*
     * add enemy to enemies list
     */
    public void add_enemy(Enemy e)
    {
        enemies.Add(e);
    }

    /*
     * Remove all the enemies buildings from the clearings, since the buildings are static, don't need to remove them from Enemy_manager
     */
    public void remove_enemy_buildings()
    {
        foreach (Building b in enemy_spawns)
        {
            b.remove_loc();
        }

        foreach (Building b in enemy_factories)
        {
            b.remove_loc();
        }
    }

    /*
     * Place the spawn buildings
     * 
     * The process will try to place a spawn alone. Currently assuming there won't be more spawns than clearings
     */
    public void place_spawns()
    {
        // get a random list of clearings to assign spawns to, prevents doubling up. Make a queue for 
        Queue<Clearing> random_clearing = new Queue<Clearing>(man.ran_man.randomize_list(man.clearings));


        // Since the building add could fail and we'll need to try adding to the next Clearing, we can't use foreach
        for (int i = 0; i < enemy_spawns.Count; i++)
        {
            spawn s = enemy_spawns[i];

            if (random_clearing.Count == 0)
            {
                Debug.LogError("no more clearings to add in random_clearing");
                return;
            }

            Clearing cl = random_clearing.Dequeue();

            bool was_successfull = cl.add_building(s);

            // if we failed to add, try again
            if (!was_successfull)
            {
                i--;
            }
        }
    }

    /*
     * Placing the factory buildings. If any of the spawns have a null location, they aren't on the map and return early
     * 
     * The factories should be 2 - 3 spaces away from it's linked spawn and not contain another factory. 
     */
    public void place_factories()
    {
        // Since each factory is tied to the spawn with the same index, need to use standard for loop
        for (int i = 0; i < enemy_factories.Count; i++)
        {
            factory cur_fact = enemy_factories[i];
            spawn linked_spawn = enemy_spawns[i];
            bool was_added = false;

            if (linked_spawn.loc == null)
            {
                Debug.LogError("The Linked spawn wasn't setup, which shouldn't happen");
            }

            linked_spawn.fact = cur_fact;

            List<Location> possible_loc = man.location_by_distance(linked_spawn.loc, 2, 3);

            // Look through each location 2 or 3 away to see if there's already a factory, if not add it and break
            foreach (Location l in possible_loc)
            {
                if (l.get_buildings().OfType<factory>().ToList().Count == 0)
                {
                    was_added = l.add_building(cur_fact);
                    if (was_added)
                    {
                        break;
                    }
                }
            }


        }
    }

    // Get a dictionary of all enemies, sorted by clearing. This prevents moving an enemy twice
    public List<(Clearing, List<Enemy>)> enemies_by_clearing()
    {
        List<(Clearing, List<Enemy>)> enemy_storage_by_clearing = new(); // store the enemies at each clearing

        // Go through once to get all enemies by clearing
        foreach (Clearing c in man.clearings.ToList())
        {
            // Check if enemies have been added to the pawns list, otherwise create an empty list that will do nothing
            if (c.pawns.ContainsKey("Enemy"))
            {
                // Using a tuple for simple access to elements like a Stack (vs a dictionary)
                enemy_storage_by_clearing.Add((c, c.get_enemies()));
            }
            
        }

        return enemy_storage_by_clearing;
    }

    // March all enemies at a clearing. Return their destinations for activating arrows
    public List<Clearing> march_clearing(Clearing c, List<Enemy> enemies_at_clearing)
    {
        List<Clearing> clearings_for_arrow = new();
        foreach (Enemy e in enemies_at_clearing)
        {
            Clearing destination = e.march();
            if (destination != null)
            {
                clearings_for_arrow.Add(destination);
            }
        }

        return clearings_for_arrow;
    }

    public void create_enemies()
    {
        foreach (spawn s in enemy_spawns)
        {
            int spawn_value = spawn_const_amount + man.ran_man.d4(spawn_dice_amount).Sum();

            s.spawn_enemies(spawn_value);
        }
    }

    // For spawning enemies outside of the spawn building, mostly for debugging. Normally should be spawned by factory
    public void spawn_enemies_separate(int amount, Location l, factory f)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject e_obj = Instantiate(man.prefabs["Enemy"]);
            Enemy e = e_obj.GetComponent<Enemy>();

            e.init(man.request_id(), l, man, this, f);
        }
    }

    /*
     * Move the spawn building, putting it at least 1 away from P
     */
    public void move_spawn()
    {
        // Get all clearings at least one distance away from player, so we don't spawn too close
        List<Clearing> available_clearings = man.location_by_distance(man.player.current_location, 2, 5).ConvertAll(x => (Clearing)x);

        // get a random list of clearings to assign spawns to, prevents doubling up
        List<Clearing> random_clearings = man.ran_man.randomize_list(available_clearings);

        // Remove the factory location(s)
        foreach(factory f in enemy_factories)
        {
            random_clearings.Remove((Clearing)f.loc);
        }

        Queue<Clearing> random_clearing_queue = new Queue<Clearing>(random_clearings);

        foreach (spawn s in enemy_spawns)
        {
            if(random_clearing_queue.Count <= 0)
            {
                Debug.LogError("ran out of clearings, shouldn't be possible");
                return;
            }

            s.remove_loc();
            
            // go through random_clearing to find an available slot, in theory there should be one available as I'm only planning for one building, but this is just in case, stop if we run out of clearings. 
            bool add_building_success = false;
            while (!add_building_success || random_clearing_queue.Count <= 0)
            {
                add_building_success = random_clearing_queue.Dequeue().add_building(s);
            }    
            
            // if we building didn't add the building, throw an error
            if(!add_building_success)
            {
                Debug.LogError("building failed to add, did we run out of clearings? random_clearing.Count: " + random_clearing_queue.Count);
            }

        }
    }

    public void inc_score(int x)
    {
        score += x;
    }

    public void reset_turn_actions()
    {
        actions_this_turn = 0;
    }

    // For debugging, fill sstate_order with sstate_fill up to amount
    public void debug_bag_fill(string sstate_fill, int amount)
    {
        sstate_order = new();

        for (int i = 0; i < amount; i++)
        {
            sstate_order.Add(sstate_fill);
        }
    }

    public List<Building> get_buildings()
    {
        List<Building> return_buildings = new();

        return_buildings.AddRange(enemy_spawns.Cast<Building>());
        return_buildings.AddRange(enemy_factories.Cast<Building>());

        return return_buildings;
    }
}
