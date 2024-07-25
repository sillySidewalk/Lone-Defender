using System.Collections;
using System.Collections.Generic;
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

    /*
     * Return a list of valid locations (clearings or forests)
     */
    public abstract List<Location> possible_moves();

    /*
     * When implemented by subclasses, will add their pawn type to the respective list of the Location to the respective List (enemies to enemy_pawns, player to player_pawns, etc) and then call move_position()
     */
    public abstract void move(Location new_loc);

    // Because enemy pawns and player pawns need to be added to separate lists, must be implemented differently
    public void move_position(Location new_loc)
    {
        current_location = new_loc;
        transform.position = new_loc.enemy_position.transform.position;
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
