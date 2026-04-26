using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * A location has spots for things to show, this defines how it will display
 * 
 * Displays the object in the center
 */
public class Location_display : MonoBehaviour
{
    [SerializeField] protected Location loc;
    protected List<int> display_positions;
    [SerializeField] protected SpriteRenderer sprite_rend;
    [SerializeField] protected Color original_color;
    [SerializeField] protected Game_piece gp;
    
    //protected Dictionary<string, List<int>> position_by_location = new (); // The string is each type of loction, the list is each position it wants (for most it's just one)
    //[SerializeField] protected List<string> position_by_location_keys = new();
    //[SerializeField] protected List<List<int>> position_by_location_values = new();

    public virtual void init(Location l)
    {
        loc = l;
        sprite_rend = GetComponent<SpriteRenderer>();
        if(sprite_rend != null)
        {
            original_color = sprite_rend.color;
        }
    }

    /*
     * For the given location type, setup self at the positions that this Location_display chose
     */
    
    public void set_loc(Location given_loc)
    {
        /*
        List<GameObject> slots = given_loc.request_pos(display_positions, null);

        //since the default case is just one slot, take the 0th element
        GameObject slot = slots[0];

        gameObject.transform.position = slot.transform.position;
        */
    }
    

    public virtual void add_game_piece(Game_piece go)
    {
        go.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, go.transform.position.z); // Put the object at the center, but keep their original Z

        gp = go;
    }

    public virtual void remove_game_piece(Game_piece go)
    {
        gp = null;
    }

    public virtual void highlight_display_if_token(string hex_color)
    {
        if(gp == null)
        {
            return;
        }

        Color new_color;

        if (ColorUtility.TryParseHtmlString("#fff700", out new_color))
        {
            sprite_rend.color = new_color;
        }
        else
        {
            Debug.LogError("Invalid hex color code: " + hex_color);
        }
    }

    public virtual void highlight_display(string hex_color)
    {
        Color new_color;

        if (ColorUtility.TryParseHtmlString("#fff700", out new_color))
        {
            sprite_rend.color = new_color;
        }
        else
        {
            Debug.LogError("Invalid hex color code: " + hex_color);
        }
    }

    public virtual void remove_display_highlight()
    {
        sprite_rend.color = original_color;
    }

    public Game_piece get_game_piece()
    {
        return gp;
    }


}
