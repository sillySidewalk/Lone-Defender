using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Enemy will perform special actions.
 * 
 * Examples:
 *      setting traps
 *      
 */
public class enemy_event : auto_exit_sub_state
{
    public override string called_next_state => throw new System.NotImplementedException();

    public override string sub_state_name => "enemy_event";

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
        Debug.Log("need to implement enemy_event state sub_state_work");
    }
}
