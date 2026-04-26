using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

/*
 * Many LD_events involve adding token to several random locations, and then the tokens doing something, so I'll make a sub class
 */
public class token_add_event : LD_event
{
    [SerializeField] protected string event_token_name; // The token used by the event
    [SerializeField] protected int min_locs; // The range of number of locations that tokens should be added
    [SerializeField] protected int max_locs; // max is inclusive
    [SerializeField] protected List<LD_token> tokens = new List<LD_token>(); // for storing tokens added during event
    //[SerializeField] protected List<int> locs_added; // the locations that tokens have been added to


    public override void start_event()
    {
        tokens = add_token_to_locs(min_locs, max_locs, event_token_name);
    }

    public override void end_event()
    {
        foreach (LD_token t in tokens)
        {
            t.loc.remove_token(t, "event");
        }
    }

    
}
