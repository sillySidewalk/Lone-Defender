using AYellowpaper.SerializedCollections;
using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class enemy_spawn : sub_state
{
    public override string called_next_state => throw new System.NotImplementedException();

    public override string sub_state_name { get; } = "enemy_spawn";

    public override bool loc_click_sub { get; } = false;

    [SerializeField] protected float animation_timer = 0; // for tracking how much time has passed
    [SerializeField] protected bool is_animating = false;
    [SerializeField] protected List<(Clearing, List<Enemy>)> enemy_storage_by_clearing;
    [SerializeField] protected Clearing current_march_clearing = null;
    [SerializeField] protected List<Enemy> current_enemies = null;
    [SerializeField] protected List<Clearing> current_arrows = null;
    [SerializeField] protected int current_step = -1; // This way the first steps() call makes it 0
    

    public override void call(string sub_state_name)
    {
        Debug.LogError("This state shouldn't be called");
    }

    public override void init()
    {
        
    }
    public override void end_state()
    {

    }

    public override void start_state()
    {
        enemy_storage_by_clearing = e_man.enemies_by_clearing();

        steps(); // start the animations        
    }


    public override void loc_click(Location loc)
    {
        Debug.LogError("Enemy sub_state shouldn't have click");
    }

    // To keep track of the flow of the state. Shows the order of the steps. We start at -1, so the first increment makes it 0 
    protected void steps()
    {
        current_step++;

        if (current_step == 0)
        {
            move_spawn_building_step();
        }
        else if (current_step == 1)
        {
            check_next_animating();
        }
        else if (current_step == 2)
        {
            spawn_enemies();
        }
        else if (current_step == 3)
        {
            finish_steps();
        }
        else
        {
            Debug.LogError("current_step is greater than all steps. current_steps: " + current_step);
        }
        
    }

    protected void move_spawn_building_step()
    {
        e_man.move_spawn();

        steps();
    }


    protected void finish_steps()
    {
        current_step = -1;

        string next_state = e_man.get_next();

        man.request_change_sub_state(next_state);
    }


    public void spawn_enemies()
    {
        foreach (spawn s in e_man.enemy_spawns)
        {
            int spawn_value = e_man.spawn_const_amount + man.ran_man.d4(e_man.spawn_dice_amount).Sum();

            s.spawn_enemies(spawn_value);
        }

        steps();
    }


    // check if any more locations need to be marched, else spawn enemies
    public void check_next_animating()
    {
        if(enemy_storage_by_clearing.Count > 0)
        {
            prepare_animating_variables();

            // If current_arrows is null, it's because there's no enemies moving
            /*
            if(current_arrows.Count == 0) // because march_step never gets called, this breaks the flow
            {
                return;
            } 
            */

            start_animating();
        }
        else
        {
            steps();
        }
    }

    // Setup current_march_clearing, current_enemies, and current_arrows
    protected void prepare_animating_variables()
    {
        (current_march_clearing, current_enemies) = enemy_storage_by_clearing[enemy_storage_by_clearing.Count - 1];
        enemy_storage_by_clearing.RemoveAt(enemy_storage_by_clearing.Count - 1); // Since order doesn't matter, removing from end is faster

        current_arrows = e_man.march_clearing(current_march_clearing, current_enemies);
    }

    protected void end_animating()
    {
        is_animating = false;
        current_march_clearing.activate_arrow(false, current_arrows);
    }
    
    public void start_animating()
    {
        // If there's on arrows, nobody moved, so we just want skip animating
        if(current_arrows.Count == 0)
        {
            animation_timer = man.enemy_march_anim_time;
        }
        else
        {
            animation_timer = 0;
            current_march_clearing.activate_arrow(true, current_arrows);
        }

        // we always want this so that march_step will move to check_next_animating()
        is_animating = true;
        
    }

    /*
     * Update the timer. When timer is done, deactivate the arrow and march the next clearing
     * 
     */
    protected void march_step()
    {
        animation_timer += Time.deltaTime;

        if (animation_timer >= man.enemy_march_anim_time)
        {
            end_animating();

            check_next_animating();
        }
    }

    private void Update()
    {
        if (is_animating)
        {
            march_step();
        }
        
    }

}
