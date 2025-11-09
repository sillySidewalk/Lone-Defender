using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class factory : Building
{
    public override Dictionary<string, List<int>> display_position
    {
        get
        {
            return new Dictionary<string, List<int>>
            {
                { "Clearing", new List<int>(){ 0 } },
            };
        }
    }

    /*
    public override Location_display get_location_display(string location_type)
    {
        Debug.LogError("Factory needs to implement get_location_display");

        return null;
    }
    //*/
}
