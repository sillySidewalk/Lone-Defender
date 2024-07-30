using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class  sub_state : MonoBehaviour
{
    [SerializeField] protected Manager man;
    protected string state_name; // The name that will be used in the Manager's state dictionaries

    public abstract void start_state();

    public abstract void end_state();
}
