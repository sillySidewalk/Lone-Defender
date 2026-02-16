using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/*
 * Transition to enemy's turn
 * 
 * Because this has to change game state instead of substate, not making this an auto_exit_sub_state
 */

public class player_end_turn : sub_state
{
    public override string called_next_state => throw new System.NotImplementedException();

    public override string sub_state_name => "player_end_turn";

    public override bool loc_click_sub => false;

    public override void call(string sub_state_name)
    {
        Debug.LogError("player_end_turn should not be called");
    }

    public override void end_state()
    {
        on_end_sstate.Invoke();
    }

    public override void init()
    {

    }

    public override void loc_click(Location loc)
    {
        Debug.LogError("player_end_turn should not be clicked");
    }

    public override void start_state()
    {
        man.request_change_state("Enemy_turn_state");
    }


    // Go through subscribed end turn effects


}
