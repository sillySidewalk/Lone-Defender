using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class game_state : MonoBehaviour
{
    public Manager man;
    public abstract string game_state_name { get; }

    public abstract void start_state();

    public abstract void end_state();

    public abstract void init();
}
