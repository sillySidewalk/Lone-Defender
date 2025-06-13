using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

/*
 *  Display has multiple slots and when given an item places them in the next available slot (first come first serve)
 */

public class multi_fcfs_display : Location_display
{
    //protected int display_count; // how many displays there are
    protected GameObject display_prefab; // The Location_display to use for each slot
    [SerializeField] protected List<Location_display> displays = new();
    [SerializeField] protected List<Game_piece>  tracked_go; // To keep track of which displays are already used



    /* Old version
    public override void init(List<int> positions)
    {
        for(int i = 0; i < display_count; i++)
        {
            GameObject new_display_go = Instantiate(display_prefab);
            Location_display new_display = new_display_go.GetComponent<Location_display>();
            displays.Add(new_display);
        }

        for(int i = 0;i < display_count; i++)
        {
            tracked_go.Add(null);
        }
    }
    */


    public override void init(Location l)
    {
        base.init(l);
        init_sub_displays(l);
        //int display_count = displays.Count;
        tracked_go = new List<Game_piece>(new Game_piece[displays.Count]);
    }

    protected void init_sub_displays(Location l)
    {
        foreach(Location_display d in displays)
        {
            d.init(l);
        }
    }


    /*
     * return the first available open slot, return -1 if none are available
     */
    protected int get_available_slot()
    {
        for(int i = 0; i < displays.Count;i++)
        {
            if (tracked_go[i] == null)  
            {
                return i;
            }
        }

        // if we made it this far, there's no slots open
        return -1;
    }

    public override void add_game_piece(Game_piece go)
    {
        int open_slot = get_available_slot();
        if(open_slot == -1)
        {
            Debug.LogError("No slot available for new GameObject Loc: " + loc.get_id());
            return;
        }

        tracked_go[open_slot] = go;
        displays[open_slot].add_game_piece(go);
    }

    public override void remove_game_piece(Game_piece go)
    {
        for(int i = 0; i < displays.Count; i++)
        {
            if (tracked_go[i] == go)
            {
                displays[i].remove_game_piece(go);
                tracked_go[i] = null;
            }
        }
    }
}
