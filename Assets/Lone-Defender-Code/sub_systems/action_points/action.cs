using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*
 * Was thinking of making sub state actions, but for now I don't see the benefit, so I'll keep as actions for now 
 */
public abstract class action : MonoBehaviour
{
    protected int cost;

    public int get_cost()
    {
        return cost;
    }

}
