using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



/*
 * Enemy_manager holds all enemy logic. Enemy turns will simply call Enemy_manager functions
 */

public class Enemy_manager : MonoBehaviour
{
    public Manager man;
    public List<Enemy> enemies = new();
    public List<spawn> enemy_spawns; // Because there are only 2 types of buildings and a spawn is always attached to a factory, easier to just have a list of each
    public List<factory> enemy_factories;
    protected int spawn_factory_amount = 1; // How many spawns and factories to make
    protected List<int> spawn_starting_clearing = new List<int>() { 1 }; // Which clearing the given spawn should start in 
    protected List<int> factory_starting_clearing = new List<int> { 10 };
    List<sub_state> sub_states; // The possible turn types
    List<sub_state> sstate_bag; // The bag to be drawn from, can have multiple occurances of a given sub_state
    Queue<sub_state> sstate_order; // The order that turn types will occur

    public int spawn_const_amount { get; } = 1; // When spawning, the amount of enemies is based on spawn_const_amount + (spawn_dice_amount)d4
    public int spawn_dice_amount { get; } = 1;

    public void init()
    {
        init_buildings();
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

    /*
     * The order of enemy turn types. After each player turn, the enemy get the next one of these.
     */
    protected void get_order()
    { 
        List<sub_state> order = man.ran_man.randomize_list(sstate_bag);

        // Add the scoring every 5 - 7 turns
        int scoring_turn = man.ran_man.rnd.Next(5, 8);

        order.Insert(scoring_turn, man.sub_states["scoring"]);

        sstate_order = new Queue<sub_state>(order);

    }

    /*
     * Get the next state and remove it from sstate_order
     */
    public string get_next()
    {
        return sstate_order.Dequeue().sub_state_name;
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


    /*
     * We get all enemies and store them by their clearing, so that they don't march twice
     */
    public IEnumerator march_by_clearing()
    {
        SerializedDictionary<Clearing, List<Enemy>> enemy_storage = new(); // store the enemies at each clearing
        HashSet<Clearing> clearing_set = new(); // clearings for marching arrow

        // Go through once to get all enemies by clearing
        foreach (Clearing c in man.clearings.ToList())
        {
            // Check if enemies have been added to the pawns list, otherwise create an empty list that will do nothing
            if (c.pawns.ContainsKey("Enemy"))
            {
                enemy_storage.Add(c, c.get_enemies());
            }
            else
            {
                enemy_storage.Add(c, new List<Enemy>());
            }
        }

        // Go through each clearing and March all the enemies and activate arrows
        foreach(var item in enemy_storage) 
        {
            foreach (Enemy e in item.Value)
            {
                Clearing next = e.march();
                if (next != null)
                {
                    clearing_set.Add(next);
                }
            }

            if (clearing_set.Count > 0)
            {
                item.Key.activate_arrow(true, clearing_set.ToList());

                yield return new WaitForSeconds(man.enemy_march_anim_time);

                item.Key.activate_arrow(false, clearing_set.ToList());
            }

            clearing_set.Clear();
        }
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
}
