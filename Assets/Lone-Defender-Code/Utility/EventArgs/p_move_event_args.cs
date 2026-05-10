using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class p_move_event_args : EventArgs
{
    public Location start_move { get; set; }
    public Location end_move { get; set; }
}
