using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class enemy_spawn : sub_state
{
    public override string next_state => throw new System.NotImplementedException();

    public override string sub_state_name { get; } = "enemy_spawn";

    public override bool loc_click_sub { get; } = false;

    public override void call()
    {
        Debug.LogError("This state shouldn't be called");
    }

    public override void init()
    {
        
    }

    public override void start_state()
    {
        StartCoroutine(march_by_clearing());

        foreach (spawn s in e_man.enemy_spawns)
        {
            int spawn_value = e_man.spawn_const_amount + man.ran_man.d4(e_man.spawn_dice_amount).Sum();

            s.spawn_enemies(spawn_value);
        }
    }

    public override void end_state()
    {
        
    }

    public override void loc_click(Location loc)
    {
        throw new System.NotImplementedException();
    }

    protected IEnumerator march_by_clearing()
    {
        //Debug.Log("start march_by_clearing");

        HashSet<Clearing> clearing_set = new();
        foreach (Clearing c in man.clearings.ToList())
        {
            

            foreach (Enemy e in c.enemy_pawns.ToList())
            {
                //Debug.Log("Clearing: " + c.get_id());
                //Debug.Log("enemy.id: " + e.id);

                Clearing next = e.march();
                //Debug.Log("next: " + next.get_id());
                if (next != null)
                {
                    clearing_set.Add(next);
                }
            }

            

            if(clearing_set.Count > 0)
            {
                //Debug.Log("clearing_set1: " + string.Join(", ", clearing_set.Select(c => c.get_id())));
                
                c.activate_arrow(true, clearing_set.ToList());

                //Debug.Log("clearing_set2: " + string.Join(", ", clearing_set.Select(c => c.get_id())));

                yield return new WaitForSeconds(man.enemy_march_anim_time);

                //Debug.Log("clearing_set3: " + string.Join(", ", clearing_set.Select(c => c.get_id())));

                c.activate_arrow(false, clearing_set.ToList());
                //Debug.Log("clearing_set4: " + string.Join(", ", clearing_set.Select(c => c.get_id())));
            }

            clearing_set.Clear();
        }
    }




}
