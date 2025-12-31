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
    
    

    public override void call(string sub_state_name)
    {
        Debug.LogError("This state shouldn't be called");
    }

    public override void loc_click(Location loc)
    {
        Debug.LogError("This state shouldn't be clicked");
    }

    public override void init()
    {
        
    }

    public override void start_state()
    {
        e_man.remove_enemy_buildings();

        e_man.place_spawns();
        e_man.place_factories();
    }

    public override void end_state()
    {
        
    }
  
}
