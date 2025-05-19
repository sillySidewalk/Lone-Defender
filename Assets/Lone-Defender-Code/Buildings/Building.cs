using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public abstract class Building : Game_piece
{
    [SerializeField] protected int _id;
    public int id { get { return _id; } }
    public Location loc; // The location of the buildings
    //[SerializeField] protected Manager man;
    protected Enemy_manager e_man;



    public void init(int init_id, Manager init_man, Enemy_manager init_e_man)
    {
        _id = init_id;
        man = init_man;
        e_man = init_e_man;
    }

    // Remove self from the loc, if there is one
    public void remove_loc()
    {
        if(loc != null)
        {
            loc.remove_building(this);
        }
    }
}
