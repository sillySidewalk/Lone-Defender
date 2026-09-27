using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * repairs damage to enemy spawn and factory
 */

public class enemy_repair : auto_exit_state
{
    public override string called_next_state => throw new System.NotImplementedException();

    public override string state_name => "enemy_repair";

    public override bool loc_click_sub => false;

    public override void init()
    {
       
    }

    public override void loc_click(Location loc)
    {
        throw new System.NotImplementedException();
    }

    protected override void state_work()
    {
        List<Building> buildings = e_man.get_buildings();

        foreach (Building b in buildings)
        {
            b.repair();
        }
    }
}
