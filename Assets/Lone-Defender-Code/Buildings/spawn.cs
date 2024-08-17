using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn : Building
{
    public factory fact; // The factory these units will move towards
    

    public void spawn_enemies(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject e_obj = Instantiate(man.prefabs["Enemy"]);
            Enemy e = e_obj.GetComponent<Enemy>();

            e.init(man.request_id(), loc, man, e_man, fact);
        }
    }
}
