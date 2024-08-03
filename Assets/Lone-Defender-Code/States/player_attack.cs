using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class player_attack : sub_state
{
    [SerializeField] protected TextMeshProUGUI btn_text;
    List<string> button_texts = new() { "Attack", "End Atk" };
    List<Location> possible_attack_locs;


    public override string next_state { get; } = "player_choose";

    public override string sub_state_name { get; } = "player_attack";

    public override bool loc_click_sub { get; } = true;

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
            p_turn.selected_pawn.attack_clearing((Clearing)loc);
        }
        
    }

    public override void call()
    {
        man.change_sub_state(next_state);
    }

    void update_locations()
    {
        Player cur_p = p_turn.selected_pawn;
        possible_attack_locs = man.location_by_distance(cur_p.current_location, 0, cur_p.atk_distance);
    }


}
