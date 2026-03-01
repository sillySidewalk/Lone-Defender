using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class factory : Building
{

    public bool move()
    {
        // Get all clearings at least one distance away from player, so we don't spawn too close
        List<Clearing> available_clearings = man.location_by_distance(man.player.current_location, 2, 5).ConvertAll(x => (Clearing)x);

        // get a random list of clearings to assign spawns to, prevents doubling up
        List<Clearing> random_clearings = man.ran_man.randomize_list(available_clearings);

        // Remove the spawn building location(s)
        foreach (spawn s in e_man.enemy_spawns)
        {
            random_clearings.Remove((Clearing)s.loc);
        }

        // Remove any other factory locations
        foreach (factory f in e_man.enemy_factories)
        {
            // Don't do this for yourself
            if(f == this)
            {
                continue;
            }

            random_clearings.Remove((Clearing)f.loc);
        }

    Queue<Clearing> random_clearing_queue = new Queue<Clearing>(random_clearings);

        
        if (random_clearing_queue.Count <= 0)
        {
            Debug.LogError("ran out of clearings, shouldn't be possible");
            return false;
        }

        remove_loc();

        // go through random_clearing to find an available slot, in theory there should be one available as I'm only planning for one building, but this is just in case, stop if we run out of clearings. 
        bool add_building_success = false;
        while (!add_building_success || random_clearing_queue.Count <= 0)
        {
            add_building_success = random_clearing_queue.Dequeue().add_building(this);
        }

        // if we building didn't add the building, throw an error
        if (!add_building_success)
        {
            Debug.LogError("building failed to add, did we run out of clearings? random_clearing.Count: " + random_clearing_queue.Count);
            return false;
        }

         return true;
    }
}
