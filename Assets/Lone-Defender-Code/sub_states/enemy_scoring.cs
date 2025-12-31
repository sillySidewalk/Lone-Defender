using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class enemy_scoring : sub_state
{
    public override string next_state { get; } = "";

    public override string sub_state_name { get; } = "enemy_scoring";

    public override bool loc_click_sub { get; } = false;

    // Shouldn't be called
    public override void call(string sub_state_name)
    {
        Debug.LogError("Enemy sub_state shouldn't be called");
    }

    /*
     * For each Clearing, enemy scores points based on number of corruption tokens
     *      6+: 7 points
     *      4-5: 3 points
     *      2-3: 1 point
     * Then remove corruption tokens
     */
    public override void start_state()
    {
        foreach(Clearing c in man.clearings)
        {
            if (!c.tokens.ContainsKey("corruption_token"))
            {
                continue;
            }

            int corruption_count = c.tokens["corruption_token"].Count;

            if(corruption_count >= 6)
            {
                man.enemy_man.inc_score(7);
            }
            else if(corruption_count >= 4)
            {
                man.enemy_man.inc_score(3);
            }
            else if(corruption_count >= 2)
            {
                man.enemy_man.inc_score(1);
            }

            // Now remove all tokens

            for(int i = c.tokens["corruption_token"].Count-1; i >= 0; i--)
            {
                LD_token t = c.tokens["corruption_token"][i];
                c.remove_token(t);
            }

        }
        
    }

    public override void end_state()
    {
        
    }

    public override void init()
    {
        
    }

    public override void loc_click(Location loc)
    {
        Debug.LogError("Enemy sub_state shouldn't have loc_click");
    }

    
}
