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

    public override void init()
    {
        Debug.LogError("player_choose init() needs to be implemented");
    }

    public override void end_state()
    {
        Debug.LogError("player_choose end_state() needs to be implemented");
    }

    public override void start_state()
    {
        Debug.LogError("player_choose start_state() needs to be implemented");
    }

    public override void call()
    {
        Debug.LogError("player_choose call() shouldn't be called");
    }

    public override void loc_click(Location loc)
    {

    }
}
