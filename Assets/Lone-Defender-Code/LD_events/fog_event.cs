using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Not necessary, replaced by token_add_event
// Fog gets added to some locations
public class fog_event : token_add_event
{
   /*

    protected List<int> fog_locs; // save the locations with fog for easy removal later

    protected LD_token fog_token;

    // Add fog tokens to 2-4 locations
    public override void start_event()
    {
        fog_locs = add_token_to_locs(2, 4, fog_token);
    }

    public override void end_event()
    {
        Location l;

        foreach (int loc in fog_locs)
        {
            l = man.clearings[loc];

            l.remove_token(fog_token);
        }
    }
   */
}
