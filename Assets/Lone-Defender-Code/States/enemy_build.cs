using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/*
 * Enemy will remove all current buildings and move them put them somewhere else. The number of building will remain static. P can damage buildings, which will reduce their effeciency, but they won't be removed
 */
public class enemy_build : sub_state
{
    public override string next_state => throw new System.NotImplementedException();

    public override string sub_state_name { get; } = "enemy_build";

    public override bool loc_click_sub { get; } = false;
    
    

    public override void call()
    {
        Debug.LogError("This state shouldn't be called");
    }

    public override void loc_click(Location loc)
    {
        Debug.LogError("This state shouldn't be called");
    }

    public override void init()
    {
        
    }

    public override void start_state()
    {
        remove_enemy_buildings();

        place_spawns();
        place_factories();
    }

    public override void end_state()
    {
        
    }


    /*
     * Remove all the enemies buildings from the clearings, since the buildings are static, don't need to remove them from Enemy_manager
     */
    void remove_enemy_buildings()
    {
        foreach(Building b in e_man.enemy_spawns)
        {
            b.remove_loc();
        }

        foreach(Building b in e_man.enemy_factories)
        {
            b.remove_loc();
        }    
    }


    /*
     * Place the spawn buildings
     * 
     * The process will try to place a spawn alone. Currently assuming there won't be more spawns than clearings
     */
    void place_spawns()
    {
        // get a random list of clearings to assign spawns to, prevents doubling up. Make a queue for 
        Queue<Clearing> random_clearing = new Queue<Clearing>(man.ran_man.randomize_list(man.clearings));
        

        // Since the building add could fail and we'll need to try adding to the next Clearing, we can't use foreach
        for(int i = 0; i < e_man.enemy_spawns.Count; i++)
        {
            spawn s = e_man.enemy_spawns[i];

            if(random_clearing.Count == 0)
            {
                Debug.LogError("no more clearings to add in random_clearing");
                return;
            }

            Clearing cl = random_clearing.Dequeue();

            bool did_add = cl.add_building(s);

            // if we failed to add, try again
            if(!did_add)
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
    void place_factories()
    {
        // Since each factory is tied to the spawn with the same index, need to use standard for loop
        for(int i = 0; i < e_man.enemy_factories.Count; i++)
        {
            factory cur_fact = e_man.enemy_factories[i];
            spawn linked_spawn = e_man.enemy_spawns[i];
            bool was_added = false;

            if(linked_spawn.loc == null)
            {
                Debug.LogError("The Linked spawn wasn't setup, which shouldn't happen");
            }
            
            linked_spawn.fact = cur_fact;

            List<Location> possible_loc = man.location_by_distance(linked_spawn.loc, 2, 3);
            
            // Look through each location 2 or 3 away to see if there's already a factory, if not add it and break
            foreach(Location l in possible_loc)
            { 
                if (l.buildings.OfType<factory>().ToList().Count == 0)
                {
                    was_added = l.add_building(cur_fact);
                    if(was_added)
                    {
                        break;
                    }
                }
            }


        }
    }

    

    
}
