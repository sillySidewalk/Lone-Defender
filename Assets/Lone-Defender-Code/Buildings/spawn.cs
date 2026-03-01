using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn : Building
{
    public factory fact; // The factory these units will move towards

    public override Dictionary<string, List<int>> display_position
    {
        get
        {
            return new Dictionary<string, List<int>>
            {
                { "Clearing", new List<int>(){ 0 } },
            };
        }
    }

    public override void init_damage_threshold()
    {
        damage_threshold = new List<int> { 3, 3, 3 };
    }

    
    public void spawn_enemies(int amount)
    {
        // reduce spawn by damage
        amount -= damage;

        for (int i = 0; i < amount; i++)
        {
            GameObject e_obj = Instantiate(man.prefabs["Enemy"]);
            Enemy e = e_obj.GetComponent<Enemy>();
            e_obj.transform.parent = man.enemy_holder.transform; // Make all enemies children of the enemy_holder

            e.init(man.request_id(), loc, man, e_man, fact);
        }
    }
}
