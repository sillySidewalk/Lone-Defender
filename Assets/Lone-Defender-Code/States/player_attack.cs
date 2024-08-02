using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class player_attack : sub_state
{
    [SerializeField] protected TextMeshProUGUI move_btn_text;
    List<string> button_texts = new() { "Attack", "End Atk" };


    public override string next_state { get; } = "player_choose";

    public override string sub_state_name { get; } = "player_attack";

    public override bool loc_click_sub { get; } = true;

    public override void start_state()
    {
        Debug.Log("player_attack start_state needs implementation");
    }

    public override void end_state()
    {
        Debug.Log("player_attack end_state needs implementation");
    }

    public override void loc_click(Location loc)
    {
        Debug.Log("player_attack loc_click needs implementation");
    }

    public override void call()
    {
        man.change_sub_state(next_state);
    }


}
