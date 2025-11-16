using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class stealth_system : MonoBehaviour
{
    Manager man;
    Player p;
    protected int current_stealth;
    protected int max_stealth;

    


    /*
     * initialize system with stats from character type.
     */
    public void init()
    {
        current_stealth = p.starting_stealth;
        max_stealth = p.max_stealth;

        p.stealth_sys = this;
    }

    public void attack_update_stealth()
    {
        update_stealth(-1);
    }


    protected void update_stealth(int value)
    {
        current_stealth += value;

        Mathf.Clamp(current_stealth, 0, max_stealth);
    }

    // Reduce retaliation dice by stealth value
    public int reduce_retal_dice(int retal_dice)
    {
        int ret_dice = retal_dice - (current_stealth * 3);

        return ret_dice;
    }
}
