using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 
 */
public class UI_formatter : MonoBehaviour
{
    public string convert_to_ratio(int current, int max)
    {
        string ratio = current.ToString() + "/" + max.ToString();

        return ratio;
    }

    // Remove "enemy_" at the beginning and take the first 3 letters
    public string enemy_state_abrev(string state_name)
    {
        string abr = state_name.Substring("enemy_".Length);

        abr = abr.Substring(0, 4);

        return abr;
    }
    

    public string add_bracket(int x)
    {
        string backeted = "(" + x.ToString() + ")";

        return backeted;
    }
}
