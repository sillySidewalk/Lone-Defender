using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class p_move_event_args : EventArgs
{
    public Clearing start_move { get; set; }
    public Clearing end_move { get; set; }
}
