using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*
 * 
 */
public class Enemy_turn_state : game_state
{
    [SerializeField] protected Enemy_manager e_man;
    [SerializeField] protected List<sub_state> sub_states; // The possible turn types
    

    //[SerializeField] public List<Enemy> enemies = new();
    //[SerializeField] public List<spawn> enemy_spawns; // Because there are only 2 types of buildings and a spawn is always attached to a factory, easier to just have a list of each
    //[SerializeField] public List<factory> enemy_factories;
    //[SerializeField] protected int spawn_factory_amount = 1; // There should be 1 factory per 1 spawn
    //[SerializeField] protected List<int> spawn_starting_clearing = new List<int>() { 1 }; // Which clearing the given spawn should start in 
    //[SerializeField] protected List<int> factory_starting_clearing = new List<int> { 10 };


    //public int spawn_const_amount { get; } = 1; // When spawning, the amount of enemies is based on: spawn_const_amount + (spawn_dice_amount)d4
    //public int spawn_dice_amount { get; } = 1;

    public override string game_state_name { get; } = "Enemy_turn_state";


    public override void start_state()
    {
        e_man.reset_turn_actions();
        string next = e_man.get_next();

        man.request_change_sub_state(next);

    }

    public override void end_state()
    {
        
    }

    public override void init()
    {
        
    }


    public override string get_next()
    {
        return e_man.get_next();
    }
   
}
