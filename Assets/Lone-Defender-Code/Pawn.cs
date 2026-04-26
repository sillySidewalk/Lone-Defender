using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/*
    Any kind of moving piece, like the player, player minions, enemy warriors
*/
public abstract class Pawn : Game_piece
{
    [SerializeField] protected int _id;
    public int id { get { return _id; } }
    //[SerializeField] protected Manager man;
    [SerializeField] public Location current_location;
    
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
        base.init();
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
     * When implemented by subclasses, will add their pawn type to the respective list of the Location to the respective List (enemies to enemy_pawns, player to player_pawns, etc) and then call new_loc.add_pawn(this)
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


    


}
