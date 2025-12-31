using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class  sub_state : MonoBehaviour
{
    [SerializeField] protected Manager man;
    [SerializeField] protected Enemy_manager e_man; // The game_state that the sub_state belongs to
    [SerializeField] public player_turn p_turn;
    [SerializeField] public UnityEvent on_end_sstate = new UnityEvent(); // For getting notified when a state ends
    public abstract string next_state { get; } // The state to transition to when exiting
    public abstract string sub_state_name { get; } // The name that will be used in the Manager's state dictionaries
    public abstract bool loc_click_sub { get; } // Whether the sub_state wants to recieve location_click

    public abstract void start_state();

    public abstract void end_state();

    /*
     * If call is this, usually return to player_choose. If call is not us, change into that state
     */
    public virtual void call(string call_sub_state_name)
    {
        if(call_sub_state_name != sub_state_name)
        {
            man.change_sub_state(call_sub_state_name);
        }
        else
        {
            called();
        }
    }

    // if we are called while the current state
    public virtual void called()
    {
        man.change_sub_state(next_state);
    }

    public abstract void loc_click(Location loc);

    public abstract void init();
}
