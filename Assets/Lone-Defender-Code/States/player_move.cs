using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Gets the possible move, shows the highlight circle, makes the possible locations into buttons
 */
public class player_move : sub_state
{
    public override string sub_state_name { get; } = "player_move";
    player_turn p_turn;
    Pawn selected_pawn;
    List<Location> possible_moves;
    public override bool loc_click_sub { get; } = true;

    public override void start_state()
    {
        p_turn = (player_turn)man.game_states["player_turn"];
        update_pawn_moves();
    }

    public override void end_state()
    {

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
     * Show the Possible_move_location_indicator
     */
    void hightlight_loc()
    {
        foreach (Location loc in possible_moves)
        {
            
            loc.possible_move_indicator.SetActive(true);
        }
    }

    /*
     * Remove all of the highlights
     */
    void remove_highlights()
    {
        foreach (Location loc in man.clearings)
        {
            loc.possible_move_indicator.SetActive(false);
        }

        foreach(Location loc in man.forests)
        {
            loc.possible_move_indicator.SetActive(false);
        }
    }

    /*
     * Update the current pawn and possible_moves
     * 
     * Used either after a move to update the new move locations or when the selected_pawn is changed
     */
    void update_pawn_moves()
    {
        remove_highlights();
        selected_pawn = p_turn.selected_pawn;
        possible_moves = selected_pawn.possible_moves();
        hightlight_loc();
    }
}
