using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * Play a random event. Happens at start of player turn
 */
public class LD_event_state : auto_exit_sub_state
{
    public override string called_next_state => throw new System.NotImplementedException();

    public override string sub_state_name => "LD_event";

    public override bool loc_click_sub => false;

    [SerializeField] List<LD_event> event_list;


    public override void init()
    {
        auto_next_state = "player_start_turn";

        event_list = man.events_obj.GetComponents<LD_event>().ToList();

        foreach (LD_event e in event_list)
        {
            e.init();
        }

        
    }

    protected override void sub_state_work()
    {
        int random_event = man.ran_man.random_num(0, event_list.Count-1);
        

        event_list[random_event].start_event();
    }

    public override void end_state()
    {
        
    }

    

    public override void loc_click(Location loc)
    {
        Debug.LogError("LD_event shouldn't have loc_click");
    }

    public override void call(string sub_state_name)
    {
        Debug.LogError("Enemy sub_state shouldn't be called");
    }

}
