using System.Collections;
using System.Collections.Generic;
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

    public override void end_state()
    {
        throw new System.NotImplementedException();
    }

    public override void loc_click(Location loc)
    {
        throw new System.NotImplementedException();
    }

    public override void start_state()
    {
        throw new System.NotImplementedException();
    }
}
