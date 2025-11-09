using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class player_end_turn : sub_state
{
    public override string next_state => throw new System.NotImplementedException();

    public override string sub_state_name => "player_end_turn";

    public override bool loc_click_sub => false;

    public override void call()
    {
        Debug.LogError("player_end_turn should not be called");
    }

    public override void start_state()
    {
        
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
        Debug.LogError("player_end_turn should not be called");
    }


    // Go through subscribed end turn effects

    
}
