using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class enemy_spawn : sub_state
{
    public override string next_state => throw new System.NotImplementedException();

    public override string sub_state_name { get; } = "enemy_spawn";

    public override bool loc_click_sub { get; } = false;

    public override void call()
    {
        Debug.LogError("This state shouldn't be called");
    }

    public override void init()
    {
        
    }

    public override void start_state()
    {
        foreach(spawn s in e_man.enemy_spawns)
        {
            int spawn_value = e_man.spawn_const_amount + man.ran_man.d4(e_man.spawn_dice_amount).Sum();

            s.spawn_enemies(spawn_value);
            
        }
    }

    public override void end_state()
    {
        
    }

    public override void loc_click(Location loc)
    {
        throw new System.NotImplementedException();
    }


}
