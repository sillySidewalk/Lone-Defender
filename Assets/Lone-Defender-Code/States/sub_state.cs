using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/*
 * 
 */
public abstract class  sub_state : MonoBehaviour
{
    [SerializeField] protected Manager man;
    [SerializeField] protected Enemy_manager e_man;
    [SerializeField] protected player_turn p_turn;
    [SerializeField] public game_state direct_man; // The game_state that directly manages this sub_state
    [SerializeField] public UnityEvent on_end_sstate = new UnityEvent(); // For getting notified when a state ends
    public abstract string called_next_state { get; } // The state to transition to when exiting automatically
    public abstract string sub_state_name { get; } // The name that will be used in the Manager's state dictionaries
    public abstract bool loc_click_sub { get; } // Whether the sub_state wants to recieve location_click

    protected bool auto_next = false; // if the class wants to automatically change state when it finishes it's start_state() 

    public abstract void start_state();

    public abstract void end_state();

    /*
     * If call is this, usually return to player_choose. If call is not us, change into that state
     */
    public virtual void call(string call_sub_state_name)
    {
        if(call_sub_state_name != sub_state_name)
        {
            man.request_change_sub_state(call_sub_state_name);
        }
        else
        {
            called();
        }
    }

    // if we are called while the current state
    public virtual void called()
    {
        man.request_change_sub_state(called_next_state);
    }

    public abstract void loc_click(Location loc);

    public virtual void init()
    {
        man = Manager.get_instance();
    }

    /*
     * Because several player sub_states have to check for action points before performing an action, centralize the process here
     * 
     * Implementation: call ap_process() before the action costing ap and then put the action costing ap in ap_act()
     */
    protected virtual bool ap_process(int ap_cost)
    {
        bool check_deduct_val = man.player.ap_system.check_deduct_ap(ap_cost);
        if (check_deduct_val)
        {
            ap_act();
        }
        else
        {
            Debug.Log("Not enough actions points");
            return false;
        }

        return true;
    }

    protected virtual void ap_act()
    {

    }
}
