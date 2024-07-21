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
}
