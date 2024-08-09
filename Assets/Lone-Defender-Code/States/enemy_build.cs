using System.Collections;
using System.Collections.Generic;
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
        Debug.LogError("enemy_build init() needs to be implemented");
    }

    public override void start_state()
    {
        remove_enemy_buildings();


    }

    public override void end_state()
    {
        throw new System.NotImplementedException();
    }


    /*
     * Remove all the enemies buildings from the clearings, since the buildings are static, don't need to remove them from Enemy_manager
     */
    void remove_enemy_buildings()
    {
        List<int> b_ids = new List<int>(); // building ids to remove

        foreach(Building b in e_man.enemy_spawns)
        {
            b_ids.Add(b.id);
            b.loc = null;
        }

        foreach(Building b in e_man.enemy_factory)
        {
            b_ids.Add(b.id);
            b.loc = null;
        }

        // Since enemy buildings are only in clearings
        foreach(Clearing c in man.clearings)
        {
            c.buildings.RemoveAll(bd => b_ids.Contains(bd.id));
        }
    }

    void place_spawns()
    {
        // get a random list of clearings to assign spawns to, prevents doubling up. Make a queue for 
        Queue<Clearing> random_clearing = new Queue<Clearing>(man.ran_man.randomize_list(man.clearings));

        foreach(spawn s in e_man.enemy_spawns)
        {

        }
    }

    /*
     * Placing the factory buildings. If any of the spawns have a null location, they aren't on the map and return early
     */
    void place_factories()
    {

    }

    

    
}
