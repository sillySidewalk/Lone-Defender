using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*
 * When you want the pieces to stack and have a counter
 */
public class counting_display : Location_display
{
    [SerializeField] protected Transform sprite_loc; // The location the the game piece will be, since it won't be center
    [SerializeField] protected TextMeshProUGUI count_txt;
    [SerializeField] protected string game_piece_type;


    // Currently done before in Unity UI
    public string get_display_type()
    {
        return game_piece_type;
    }
    public void set_display_type(string init_display_type)
    {
        game_piece_type = init_display_type;
    }

    public override void add_game_piece(Game_piece go)
    {
        go.transform.position = sprite_loc.position;

        loc.get_count(game_piece_type);
    }

    public override void remove_game_piece(Game_piece go)
    {
        loc.get_count(game_piece_type);
    }
}
