using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : Pawn
{
    public override Location.move_position move_pos { get; } = Location.move_position.enemy;

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
        current_location.enemy_pawns.Remove(this);
        move_position(new_loc);
    }
}
