using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
    Any kind of moving piece, like the player, player minions, enemy warriors
*/
public abstract class Pawn : MonoBehaviour
{
    Clearing current_location;


    // Return a list of valid locations (clearings or forests)
    public abstract List<Location> possible_moves();
}
