using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class  sub_state : MonoBehaviour
{
    [SerializeField] protected Manager man;
    public abstract string sub_state_name { get; } // The name that will be used in the Manager's state dictionaries
    public abstract bool loc_click_sub { get; } // Whether the sub_state wants to recieve location_click

    public abstract void start_state();

    public abstract void end_state();

    public abstract void loc_click(Location loc);
}
