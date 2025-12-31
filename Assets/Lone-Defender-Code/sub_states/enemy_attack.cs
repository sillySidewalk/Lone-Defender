using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Enemies attack in their clearing
 */
public class enemy_attack : sub_state
{
    public override string next_state => throw new System.NotImplementedException();

    public override string sub_state_name => "enemy_attack";

    public override bool loc_click_sub => false;

    public override void call(string sub_state_name)
    {
        Debug.LogError("enemy_attack sub_state shouldn't be called");
    }

    public override void end_state()
    {
        
    }

    public override void init()
    {
        
    }

    public override void loc_click(Location loc)
    {
        Debug.LogError("enemy_attack sub_state shouldn't have loc_click");
    }

    public override void start_state()
    {
        List<Clearing> clearings = man.clearings;
        foreach(Clearing c in clearings)
        {
            if(man.is_player_at(c))
            {
                int num_dice = c.get_enemies().Count * e_man.enemy_atk_dice;
                num_dice -= man.player.stealth_sys.atk_reduce_val();

                if(num_dice <= 0)
                {
                    continue;
                }

                List<int> atks = man.ran_man.d10(num_dice);

                man.attack_player(atks);
            }
        }
    }
}
