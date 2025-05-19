using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Enemies at Factories produce based on how many enemies there are
 */
public class enemy_produce : sub_state
{
    public override string next_state => "";

    public override string sub_state_name { get; } = "enemy_produce";

    public override bool loc_click_sub { get; } = false;

    public override void call()
    {
        Debug.LogError("enemy_produce shouldn't be called");
    }

    public override void start_state()
    {
        factory_produce();
    }


    public override void end_state()
    {

    }

    public override void init()
    {

    }

    public override void loc_click(Location loc)
    {
        Debug.LogError("enemy_produce shouldn't have loc_click() called");
    }

    /*
     * Produce 1 corruption for each 3 enemies
     */
    protected void factory_produce()
    {
        foreach (factory f in e_man.enemy_factories)
        {
            int produce_value = (int) Mathf.Floor(f.loc.pawns["enemy"].Count/ 3);

            for(int i = 0; i < produce_value; i++)
            {
                GameObject ct_ob = Instantiate(man.prefabs["corruption token"], new Vector3(0, 0, 0), Quaternion.identity);
                corruption_token ct = ct_ob.GetComponent<corruption_token>();

                Clearing factory_clearing = (Clearing)f.loc;
                factory_clearing.add_corruption(ct);
            }
        }
    }

}

    
