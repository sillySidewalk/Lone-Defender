using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * repairs damage to enemy spawn and factory
 */

public class enemy_repair : auto_exit_sub_state
{
    public override string called_next_state => throw new System.NotImplementedException();

    public override string sub_state_name => "enemy_repair";

    public override bool loc_click_sub => false;

    public override void end_state()
    {
        
    }

    public override void init()
    {
       
    }

    public override void loc_click(Location loc)
    {
        throw new System.NotImplementedException();
    }

    protected override void sub_state_work()
    {
        Debug.Log("implement enemy_repair sub_state_work");
    }
}
