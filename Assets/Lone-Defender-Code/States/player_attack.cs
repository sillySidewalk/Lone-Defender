using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class player_attack : sub_state
{
    [SerializeField] protected TextMeshProUGUI btn_text;
    List<string> button_texts = new() { "Attack", "End Atk" };
    List<Location> possible_attack_locs;
    


    public override string called_next_state { get; } = "player_choose";

    public override string sub_state_name { get; } = "player_attack";

    public override bool loc_click_sub { get; } = true;

    public override void init()
    {
        //Debug.LogWarning("player_attack init() needs to be implemented");
    }

    public override void start_state()
    {
        btn_text.text = button_texts[1];
        update_locations();
        man.hightlight_loc(possible_attack_locs);
    }

    public override void end_state()
    {
        btn_text.text = button_texts[0];
        man.remove_all_highlights();
    }

    public override void loc_click(Location loc)
    {
        if(possible_attack_locs.Contains(loc))
        {
            bool check_deduct_val = man.player.ap_system.check_deduct_ap(1);
            if (check_deduct_val)
            {
                p_turn.player.attack_clearing((Clearing)loc);
            }
            else
            {
                Debug.Log("Not enough actions points");
            }
        }
        
    }

    void update_locations()
    {
        Player cur_p = p_turn.player;
        possible_attack_locs = man.location_by_distance(cur_p.current_location, 0, cur_p.atk_distance);
    }


}
