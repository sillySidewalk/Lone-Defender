using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*
 * Gets the possible move, shows the highlight circle, makes the possible locations into buttons
 */
public class player_move : sub_state
{
    public override string called_next_state { get; } = "player_choose";
    public override string sub_state_name { get; } = "player_move";
    [SerializeField] Player player_pawn;
    [SerializeField] List<Location> possible_moves;
    [SerializeField] protected TextMeshProUGUI move_btn_text;
    [SerializeField] List<string> button_texts = new() { "Move", "End Move"};
    public override bool loc_click_sub { get; } = true;
    [SerializeField] protected SerializedDictionary<string, int> ap_cost = new SerializedDictionary<string, int> { { "clearing", 1 } };

    public override void init()
    {
        player_pawn = p_turn.player;
    }

    public override void start_state()
    {
        move_btn_text.text = button_texts[1];
        //p_turn = (player_turn)man.game_states["player_turn"];
        update_pawn_moves();
    }

    public override void end_state()
    {
        move_btn_text.text = button_texts[0];
        man.remove_all_highlights();
    }


    /*
     * Manager passed us a Location click, if the pawn can move there, then move
     */
    public override void loc_click(Location loc)
    {
        if(possible_moves.Contains(loc))
        {
            bool check_deduct_val = man.player.ap_system.check_deduct_ap(ap_cost["clearing"]);
            if (check_deduct_val)
            {
                player_pawn.move(loc);
            }
            else
            {
                Debug.Log("not enough Action Points");
            }
        }
        update_pawn_moves();
    }

    /*
     * Update the current pawn and possible_moves
     * 
     * Used either after a move to update the new move locations or when the selected_pawn is changed
     */
    void update_pawn_moves()
    {
        man.remove_all_highlights();
        //player_pawn = p_turn.player;
        possible_moves = player_pawn.possible_moves();
        man.hightlight_loc(possible_moves);
    }
}
