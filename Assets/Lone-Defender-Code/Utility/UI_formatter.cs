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
}
