using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Handles multiple counting_dest_displays. If there is a counting_dest_display with the same destination as Enemy, add it to that one. Otherwise add to one of the count_dest_display that's empty
 */

public class multi_stack_display : Location_display
{
    [SerializeField] protected List<counting_dest_display> displays = new();

    public override void add_game_piece(Game_piece go)
    {
        Enemy new_e = (Enemy)go;
        int factory_dest = new_e.fact.id;

        foreach(counting_dest_display d in displays)
        {
            if(d.factory_dest == factory_dest)
            {
                d.add_game_piece(go);
                return;
            }
        }

        // If we made it this far, there's isn't any display with matching destination
        foreach (counting_dest_display d in displays)
        {
            if (d.factory_dest == -1)
            {
                d.add_game_piece(go);
                return;
            }
        }

        // We have more factories than display, print error
        Debug.LogError("No available display for enemies with destination " + new_e.fact.id + ", not enough displays for the number of factories?");
    }

    public override void remove_game_piece(Game_piece go)
    {
        base.remove_game_piece(go);
    }
}
