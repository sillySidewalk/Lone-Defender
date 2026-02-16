using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*
 * Because some states want to exit after their finished. 
 * 
 * Instead of implementing start_state(), implement sub_state_work. At the end of start_state's function, request_change_sub_state is called with auto_next_state
 */
public abstract class auto_exit_sub_state : sub_state
{
    [SerializeField] protected string auto_next_state = null;



    
    public override void start_state()
    {
        determin_auto_next_state();

        sub_state_work();

        man.request_change_sub_state(auto_next_state);
        
    }

    // What actually happens in the enemy_sub_state
    protected abstract void sub_state_work();

    // Some states will need to determine at run time what their next state will be
    protected virtual void determin_auto_next_state()
    {
        auto_next_state = direct_man.get_next();
    }
}
