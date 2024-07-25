using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;



public class Clearing : Location
{

    [SerializeField] List<int> en_def_mods; // List of the Enemies Defense Modifiers, applies to each attack from player
    [SerializeField] List<int> p_atk_mods; // List of Player attack Modifiers for this location, applies to each attack from player

    /*
     * Return the sum of the Enemies Defense Modifiers
     */
    public int get_en_def_sum()
    {
        int sum = 0;

        foreach(int i in en_def_mods)
        {
            sum += i;
        }

        return sum;
    }

    public int get_p_atk_sum()
    {
        int sum = 0;

        foreach (int i in p_atk_mods)
        {
            sum += i;
        }

        return sum;
    }
}
