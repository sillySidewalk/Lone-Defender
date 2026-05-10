using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class player_healing : auto_exit_sub_state
{
    public override string called_next_state => null;

    public override string sub_state_name => nameof(player_healing);

    public override bool loc_click_sub => false;

    protected int clearing_heal_dice = 3;
    protected int clearing_heal_mod = 1; // To give P a slightly higher average than 1 per healing

    protected int forest_heal_dice = 6;
    protected int forest_heal_mod = 3;

    protected int forest_end_turn_healing = 3;


    public override void init()
    {
        base.init();

        man.sub_states[nameof(player_end_turn)].on_end_sstate += end_turn_healing;
    }

    public override void end_state()
    {
        
    }

    public override void loc_click(Location loc)
    {
        
    }

    protected override void sub_state_work()
    {
        ap_process(2);
    }

    protected override void ap_act()
    {
        heal_by_location();

    }


    /*
     * Check if P is in Clearing or Forest
     * 
     * Giving a separate function to make ap_act() clearer
     */
    protected void heal_by_location()
    {
        Location p_cur_loc = man.player.current_location;

        if(p_cur_loc is Clearing)
        {
            heal(clearing_heal_dice, clearing_heal_mod);
        }
        else if(p_cur_loc is Forest)
        {
            heal(forest_heal_dice, forest_heal_mod);
        }
        else
        {
            Debug.Log("player is not in Clearing or Forest, shouldn't be possible");
        }
    }

    protected void heal(int dice_num, int mod)
    { 
        List<int> healing_dice = man.ran_man.d10(dice_num, "Healing");

        healing_dice = man.add_mod(healing_dice, mod);

        int healing_amnt = man.count_success(healing_dice);

        man.player.adjust_health(healing_amnt);
    }

    // If P ends their turn in a forest, gain 3 health
    protected void end_turn_healing(object sender, EventArgs e)
    {
        Location p_cur_loc = man.player.current_location;

        if(p_cur_loc is Forest)
        {
            man.player.adjust_health(forest_end_turn_healing);
        }
    }
}
