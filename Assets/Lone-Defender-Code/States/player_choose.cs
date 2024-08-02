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
    public override string next_state { get; } // Since this is basically the default, it mostly doesn't default go to another sub_state

    public override void end_state()
    {
        
    }

    public override void start_state()
    {
        
    }

    public override void call()
    {
        
    }

    public override void loc_click(Location loc)
    {

    }
}
