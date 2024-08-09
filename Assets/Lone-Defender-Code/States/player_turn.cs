using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_turn : game_state
{
    public override string game_state_name { get; } = "player_turn";
    public Player selected_pawn; // The current Player pawn selected, mostly used by sub_states

    public override void start_state()
    {
        
    }

    public override void end_state()
    {
        
    }

    public override void init()
    {
        Debug.Log("Enemy_manager init() needs to be implemented");
    }
}
