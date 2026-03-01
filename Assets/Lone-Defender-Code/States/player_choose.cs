using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Player is choosing what to do, the defualt state
 */
public class player_choose : sub_state
{
    public override bool loc_click_sub { get; } = false;
    public override string sub_state_name { get; } = "player_choose";
    public override string called_next_state { get; } // Since this is basically the default, it mostly doesn't default go to another sub_state

    public override void init()
    {
        
    }

    public override void end_state()
    {
       
    }

    public override void start_state()
    {
        
    }

    // Should only be called at when ending turn. Then ask player_turn for next step to keep flow centralized
    public override void called()
    {
        string next_sub_state = direct_man.get_next();

        man.request_change_sub_state(next_sub_state);
    }

    public override void loc_click(Location loc)
    {

    }
}
