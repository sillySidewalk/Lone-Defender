using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : Pawn
{
    /*
     * Enemies can only move to clearings
     */
    public override List<Location> possible_moves()
    {
        return current_location.adjacent_locations.OfType<Clearing>().Cast<Location>().ToList();
    }

    // Add the enemy pawn to the new locations enemy_pawns list, then do everything else the same
    public override void move(Location new_loc)
    {
        Debug.Log("new_loc = " + new_loc.id);
        new_loc.enemy_pawns.Add(this);

        Debug.Log("new_loc.enemy_pawns.count" + new_loc.enemy_pawns.Count);
        move_position(new_loc);
    }
}
