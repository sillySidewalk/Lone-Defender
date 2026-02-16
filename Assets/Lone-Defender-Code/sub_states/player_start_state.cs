using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_start_state : auto_exit_sub_state
{
    public override string called_next_state => throw new System.NotImplementedException();

    public override string sub_state_name => "player_start_state";

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

    // Whatever needs to be done at the start of a player turn
    protected override void sub_state_work()
    {
        man.player.ap_system.refill_action_point();
    }
}
