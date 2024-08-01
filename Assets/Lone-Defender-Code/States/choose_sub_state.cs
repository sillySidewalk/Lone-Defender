using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Player is choosing what to do, the defualt state
 */
public class choose_sub_state : sub_state
{
    public override bool loc_click_sub { get; } = false;
    public override string sub_state_name { get; } = "choose_sub_state";

    public override void end_state()
    {
        
    }

    public override void loc_click(Location loc)
    {
        
    }

    public override void start_state()
    {
        
    }
}
