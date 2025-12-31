using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*
 * Handles controlling the enemy
 * 
 * Enemy_manager has a list of sub_states that represent what it does on a given turn. it will have a list of those sub_states that will represent how often they will happen, where they can appear more than once. It then draws from that list to determine the order of the turn types, when one is drawn it's not replaced till all are drawn
 * 
 */
public class Enemy_manager_state : game_state
{
    [SerializeField] private List<sub_state> sub_states; // The possible turn types
    [SerializeField] List<sub_state> sstate_bag; // The bag to be drawn from, can have multiple occurances of a given sub_state
    [SerializeField] List<sub_state> sstate_order; // The order that turn types will occur
    [SerializeField] public List<Enemy> enemies = new();
    [SerializeField] public List<spawn> enemy_spawns; // Because there are only 2 types of buildings and a spawn is always attached to a factory, easier to just have a list of each
    [SerializeField] public List<factory> enemy_factories;
    [SerializeField] protected int spawn_factory_amount = 1; // There should be 1 factory per 1 spawn
    [SerializeField] protected List<int> spawn_starting_clearing = new List<int>() { 1 }; // Which clearing the given spawn should start in 
    [SerializeField] protected List<int> factory_starting_clearing = new List<int> { 10 };


    public int spawn_const_amount { get; } = 1; // When spawning, the amount of enemies is based on: spawn_const_amount + (spawn_dice_amount)d4
    public int spawn_dice_amount { get; } = 1;

    public override string game_state_name { get; } = "Enemy_manager";


    /*
    public void get_order()
    {
        List<sub_state> order = man.ran_man.randomize_list(sstate_bag);

        // Add the scoring every 5 - 7 turns
        int scoring_turn = man.ran_man.rnd.Next(5, 8);

        order.Insert(scoring_turn, man.sub_states["scoring"]);

        sstate_order = order;

    }
    */

    /*
     * Get the next state and remove it from sstate_order
     */
    /*
    public string get_next()
    {
        return sstate_order.Dequeue().sub_state_name;
    }
    */

    public override void start_state()
    {
        
    }

    public override void end_state()
    {
        
    }

    public override void init()
    {
        
    }

   
}
