using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class  sub_state : MonoBehaviour
{
    [SerializeField] protected Manager man;
    [SerializeField] protected Enemy_mananger e_man; // The game_state that the sub_state belongs to
    [SerializeField] public player_turn p_turn;
    public abstract string next_state { get; } // The state to transition to when exiting
    public abstract string sub_state_name { get; } // The name that will be used in the Manager's state dictionaries
    public abstract bool loc_click_sub { get; } // Whether the sub_state wants to recieve location_click

    public abstract void start_state();

    public abstract void end_state();

    /*
     * When the same mechanism to change into this sub_state is called (usually a button), gets passed to the sub_state (usually to end the sub_state)
     */
    public abstract void call();

    public abstract void loc_click(Location loc);
}
