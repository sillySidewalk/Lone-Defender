using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Any object that would be a piece on the board
 */
public abstract class Game_piece : MonoBehaviour
{
    [SerializeField] protected Manager man;
    [SerializeField] protected string ld_name; // The name of Location_display, get prefab from Manager
    //[SerializeField] protected GameObject ld_prefab;

    public virtual Dictionary<string, List<int>> display_position { get; } // String is the location type, List<int> is the positions it uses on the location type

    /*
     * Manager will provide the locaiton_type and Game_piece will initialize the location display and hand it back
     */
    public virtual Location_display get_location_display(string location_type) // return the initialized Location_display
    {
        GameObject ld_prefab = man.prefabs[ld_name];
        if(ld_prefab == null)
        {
            Debug.LogError(ld_name + " prefab not found");

            return null;
        }

        // If it's not meant to go to that location, the display_position won't have an entry for it, so return null
        if(!display_position.ContainsKey(location_type))
        {
            return null;
        }

        // By defaut, objects only have 1 display location, so the Location_display will just use the 0th element, the multi-displays will process this differently. This way I just have a function that accepts a List as opposed to 2 types that accept an int or a list
        List<int> disp_location = display_position[location_type];

        GameObject ld_ob = Instantiate(man.prefabs[ld_name]);
        Location_display ld = ld_ob.GetComponent<Location_display>();

        //ld.init(disp_location);



        return ld;
    }
}
