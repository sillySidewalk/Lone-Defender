using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Handles multiple counting_dest_displays. If there is a counting_dest_display with the same destination as Enemy, add it to that one. Otherwise add to one of the count_dest_display that's empty
 */

public class multi_stack_display : Location_display
{
    [SerializeField] protected List<counting_dest_display> displays = new();

    public override void init(Location l)
    {
        base.init(l);
        init_sub_displays(l);

    }

    protected void init_sub_displays(Location l)
    {
        foreach(counting_dest_display d in displays)
        {
            d.init(l);
        }
    }


    public override void add_game_piece(Game_piece go)
    {
        if(go is Enemy)
        {
            add_enemy((Enemy) go);
        }
        else
        {
            Debug.LogError("need to implement adding non-enemy game pieces for multi_stack_display");
        }
    }

    protected void add_enemy(Enemy new_e)
    {
        int factory_dest = new_e.fact.id; ;

        

        // check if there are any other enemies with the same destination
        foreach(counting_dest_display d in displays)
        {
            if(d.factory_dest == factory_dest)
            {
                d.add_game_piece((Game_piece) new_e);
                return;
            }
        }

        // If we made it this far, there's isn't any display with matching destination
        foreach (counting_dest_display d in displays)
        {
            if (d.factory_dest == -1)
            {
                d.add_game_piece((Game_piece) new_e);
                return;
            }
        }

        // We have more factories than display, print error
        Debug.LogError("No available display for enemies with destination " + new_e.fact.id + ", not enough displays for the number of factories?");
    }

    public override void remove_game_piece(Game_piece go)
    {
        base.remove_game_piece(go);

        Enemy new_e = (Enemy)go;
        int factory_dest = new_e.fact.id;

        // check if there are any other enemies with the same destination
        foreach (counting_dest_display d in displays)
        {
            if (d.factory_dest == factory_dest)
            {
                d.remove_game_piece(go);
                return;
            }
        }

        // if we never found a display with matching destination, there's an issue
        Debug.LogError("Didn't find display with matching destination, but we get location to remove from pawn. Shouldn't be possible");
    }
}
