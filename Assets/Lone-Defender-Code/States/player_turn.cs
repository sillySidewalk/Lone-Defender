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
        string current_state = man.current_sub_state.sub_state_name;

        if (current_state == "player_start_state")
        {
            return "player_choose";
        }
        else if(current_state == "player_choose")
        {
            return "LD_event_state";
        }
        else if(current_state == "LD_event_state")
        {
            return "player_end_turn";
        }

            return null;
    }
}
