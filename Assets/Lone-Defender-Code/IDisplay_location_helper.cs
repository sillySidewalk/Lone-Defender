using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Used for getting the display locations for game pieces
 */
public interface IDisplay_location_helper
{
    public Dictionary<string, int> display_position { get; } // String is the location type, List<int> is the positions it uses on the location type
    //public Location_display get_location_display(); // return the initialized Location_displays
}
