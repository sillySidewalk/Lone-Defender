using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * Play a random event. Happens at start of player turn
 */
public class LD_event_state : auto_exit_state
{
    [SerializeField] protected GameObject event_ui_parent;

    public override string called_next_state => throw new System.NotImplementedException();

    public override string state_name => "LD_event_state";

    public override bool loc_click_sub => false;

    [SerializeField] List<LD_event> event_list;
    [SerializeField] LD_event current_event;


    public override void init()
    {
        //auto_next_state = "player_start_turn";

        event_list = man.events_obj.GetComponents<LD_event>().ToList();

        foreach (LD_event e in event_list)
        {
            e.init();
        }

        
    }

    // End current event, then start next event

    protected override void state_work()
    {
        
        if(current_event != null)
        {
            current_event.end_event();
        }
        
        int random_event = man.ran_man.random_num(0, event_list.Count-1);

        current_event = event_list[random_event];

        current_event.start_event();

        add_to_ui(current_event);
    }


    public override void loc_click(Location loc)
    {
        Debug.LogError("LD_event shouldn't have loc_click");
    }

    public override void call(string state_name)
    {
        Debug.LogError("Enemy state shouldn't be called");
    }

    public void add_to_ui(LD_event e)
    {
        GameObject prefab = man.prefabs["event_ui_obj"];
        GameObject instance = Instantiate(prefab);
        event_ui e_ui = instance.GetComponent<event_ui>();

        instance.transform.SetParent(event_ui_parent.transform, false);
        e_ui.init(e);
    }

}
