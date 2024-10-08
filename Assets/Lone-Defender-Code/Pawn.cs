using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/*
    Any kind of moving piece, like the player, player minions, enemy warriors
*/
public abstract class Pawn : MonoBehaviour
{
    [SerializeField] protected int _id;
    public int id { get { return _id; } }
    [SerializeField] protected Manager man;
    [SerializeField] public Location current_location;
    [SerializeField] protected int max_hp;
    [SerializeField] protected int hp;
    [SerializeField] protected int max_stealth;
    [SerializeField] protected int stealth;
    [SerializeField] protected int max_ap;// Action points
    [SerializeField] protected int ap;
    abstract public move_type m_type { get; }
    public abstract Location.move_position move_pos { get; } // where in the clearing we want to be
    

    public enum move_type
    {
        clearings,
        forests,
        clear_for // Clearings and Forests
    }

    public virtual void init(int given_id, Location _loc)
    {
        _id = given_id;
        move(_loc);
    }


    /*
     * Return a list of valid locations (clearings or forests)
     * TODO: Since there are only 3 types of moves (forests, clearings, forests & clearings), this could be replaced with a simple selector
     */
    public List<Location> possible_moves()
    { 
        if(m_type == move_type.clearings)
        {
            return current_location.adjacent_locations.OfType<Clearing>().Cast<Location>().ToList();
        }
        else if(m_type == move_type.forests)
        {
            return current_location.adjacent_locations.OfType<Forest>().Cast<Location>().ToList();
        }
        else // m_type == move_type.clear_for
        {
            return current_location.adjacent_locations;
        }

    }

    /*
     * When implemented by subclasses, will add their pawn type to the respective list of the Location to the respective List (enemies to enemy_pawns, player to player_pawns, etc) and then call move_position()
     */
    public virtual void move(Location new_loc)
    {
        if(current_location != null)
        {
            current_location.remove_pawn(this);
        }
        current_location = new_loc;
        new_loc.add_pawn(this);
    }

    // Because enemy pawns and player pawns need to be added to separate lists, must be implemented differently. Call in move to set Pawns values
    /*
    protected void move_position(Location new_loc)
    {
        
    }
    //*/

    /*
     * Relative change to health, adding or subtracting
     */
    public void adjust_health(int value)
    {
        hp += value;
        Mathf.Clamp(hp, 0, max_hp);
    }

    /*
     * Relative change to stealth, adding or subtracting
     */
    public void adjust_stealth(int value)
    {
        stealth += value;
        Mathf.Clamp(stealth, 0, max_stealth);
    }

    /*
     * Relative change to Action Points, adding or subtracting
     */
    public void adjust_ap(int value)
    {
        ap += value;
        Mathf.Clamp(ap, 0, max_ap);
    }


}
