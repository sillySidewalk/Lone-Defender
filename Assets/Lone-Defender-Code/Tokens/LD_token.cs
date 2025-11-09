using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LD_token : Game_piece
{
    [SerializeField] public abstract string token_name { get; }
    [SerializeField] protected Location _loc;
    [SerializeField] public Location loc { get { return _loc; } set { _loc = value; } }
    

    public void move(Location new_loc)
    {
        transform.position = new_loc.transform.position;
        loc = new_loc;
    }

    // by default, add_effect does nothing
    public virtual void add_effect()
    {

    }

    // by default, nothing will happen with remove effect.
    public virtual void remove_effect()
    {

    }

    // If token has anything to clean up
    public virtual void remove()
    {
        remove_effect();
        Destroy(this.gameObject);
    }
}
