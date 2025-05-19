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

        HashSet<Clearing> clearing_set = new();
        foreach (Clearing c in man.clearings.ToList())
        {
            

            foreach (Enemy e in c.pawns["enemy"].ConvertAll(x => (Enemy)x).ToList())
            {
                Clearing next = e.march();
                if (next != null)
                {
                    clearing_set.Add(next);
                }
            }

            if(clearing_set.Count > 0)
            {                
                c.activate_arrow(true, clearing_set.ToList());

                yield return new WaitForSeconds(man.enemy_march_anim_time);

                c.activate_arrow(false, clearing_set.ToList());
            }

            clearing_set.Clear();
        }
    }




}
