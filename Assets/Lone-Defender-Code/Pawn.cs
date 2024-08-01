using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/*
    Any kind of moving piece, like the player, player minions, enemy warriors
*/
public abstract class Pawn : MonoBehaviour
{
    [SerializeField] protected Manager man;
    [SerializeField] protected Location current_location;
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
    public abstract void move(Location new_loc);

    // Because enemy pawns and player pawns need to be added to separate lists, must be implemented differently. Call in move to set Pawns values
    protected void move_position(Location new_loc)
    {
        current_location = new_loc;
        new_loc.add_pawn(this);
    }

    /*
     * Relative change to health, adding or subtracting
     */
    public void adjust_health(int value)
    {
        hp += value;
        if(hp > max_hp)
        {
            hp = max_hp;
        }
        else if(hp < 0)
        {
            hp = 0;
        }
    }

    /*
     * Relative change to stealth, adding or subtracting
     */
    public void adjust_stealth(int value)
    {
        stealth += value;
        if (stealth > max_stealth)
        {
            stealth = max_stealth;
        }
        else if (stealth < 0)
        {
            stealth = 0;
        }
    }

    /*
     * Relative change to Action Points, adding or subtracting
     */
    public void adjust_ap(int value)
    {
        ap += value;
        if (ap > max_ap)
        {
            ap = max_ap;
        }
        else if (ap < 0)
        {
            ap = 0;
        }
    }


}
