using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Pawn
{
    /*
     * List valid locations to move to.  
     */
    public override List<Location> possible_moves()
    {
        
        if (current_location.GetType() == typeof(Clearing))
        {
            // If player is in a clearing, they can move to an ajacent Clearing or Forest
            return current_location.adjacent_locations;
        }
        else if(current_location.GetType() == typeof(Forest))
        {
            //If player is in Forest, they can move to adjacent Clearing.
            return current_location.adjacent_locations.OfType<Clearing>().Cast<Location>().ToList();
        }
        else
        {
            Debug.LogError("current_location was unknown type");
            return null;
        }    
    }

    

}
