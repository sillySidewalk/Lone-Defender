using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * take care of any last things for enemy and switch to end round
 * 
 * While it does automatically exit, it needs to change both game_state and sub_state, so it is not an auto_exit_sub_state
 */
public class enemy_end_turn : sub_state
{
    public override string called_next_state => throw new System.NotImplementedException();

    public override string sub_state_name => "enemy_end_turn";

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

    public override void start_state()
    {
        man.request_change_state("player_turn");
    }
}
