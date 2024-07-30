using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Gets the possible move, shows the highlight circle, makes the possible locations into buttons
 */
public class player_move : sub_state
{
    player_turn p_turn;
    Pawn selected_pawn;
    List<Location> possible_moves;

    public override void start_state()
    {
        p_turn = (player_turn)man.game_states["player_turn"];
        update_pawn_moves();
        hightlight_loc_on();

        
    }

    public override void end_state()
    {

    }

    /*
     * Show the Possible_move_location_indicator
     */
    void hightlight_loc_on()
    {
        foreach (Location loc in possible_moves)
        {
            loc.possible_move_indicator.SetActive(true);
        }
    }

    /*
     * Update the current pawn and possible_moves
     * 
     * Used either after a move to update the new move locations or when the selected_pawn is changed
     */
    void update_pawn_moves()
    {
        selected_pawn = p_turn.selected_pawn;
        possible_moves = selected_pawn.possible_moves();
    }
}
