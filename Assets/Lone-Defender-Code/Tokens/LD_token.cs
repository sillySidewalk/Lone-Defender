using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LD_token : Game_piece
{
    public abstract string token_name { get; }
    protected Location _loc;
    public Location loc { get { return _loc; } protected set { _loc = value; } }
    

    public void move(Location new_loc)
    {
        transform.position = new_loc.transform.position;
        loc = new_loc;
    }
}
