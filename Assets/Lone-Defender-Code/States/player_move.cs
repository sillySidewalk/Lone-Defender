using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*
 * Gets the possible move, shows the highlight circle, makes the possible locations into buttons
 */
public class player_move : sub_state
{
    public override string next_state { get; } = "player_choose";
    public override string sub_state_name { get; } = "player_move";
    Pawn selected_pawn;
    List<Location> possible_moves;
    [SerializeField] protected TextMeshProUGUI move_btn_text;
    List<string> button_texts = new() { "Move", "End Move"};
    public override bool loc_click_sub { get; } = true;

    public override void init()
    {
        Debug.LogError("player_move init() needs to be implemented");
    }

    public override void start_state()
    {
        move_btn_text.text = button_texts[1];
        p_turn = (player_turn)man.game_states["player_turn"];
        update_pawn_moves();
    }

    public override void end_state()
    {
        move_btn_text.text = button_texts[0];
        man.remove_all_highlights();
    }

    /*
     * End the sub_state and transiton back to choose_sub_state
     */
    public override void call()
    {
        man.change_sub_state(next_state);
    }


    /*
     * Manager passed us a Location click, if the pawn can move there, then move
     */
    public override void loc_click(Location loc)
    {
        if(possible_moves.Contains(loc))
        {
            selected_pawn.move(loc);
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
        selected_pawn = p_turn.selected_pawn;
        possible_moves = selected_pawn.possible_moves();
        man.hightlight_loc(possible_moves);
    }
}
