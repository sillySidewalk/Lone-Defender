using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_turn : game_state
{
    public override string game_state_name { get; } = "player_turn";
    public Player player; // The current Player pawn selected, mostly used by sub_states

    public override void start_state()
    {
        man.request_change_sub_state("player_start_state");
    }

    public override void end_state()
    {
        
    }

    public override void init()
    {
        //Debug.LogWarning("player_turn init() needs to be implemented");
    }

    public override string get_next()
    {
        if(man.current_sub_state.name == "player_start_state")
        {
            return "player_choose";
        }

        return null;
    }
}
