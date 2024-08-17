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
public class Enemy_manager : game_state
{
    List<sub_state> sub_states; // The possible turn types
    List<sub_state> sstate_bag; // The bag to be drawn from, can have multiple occurances of a given sub_state
    Queue<sub_state> sstate_order; // The order that turn types will occur
    public List<Enemy> enemies = new();
    public List<spawn> enemy_spawns; // Because there are only 2 types of buildings and a spawn is always attached to a factory, easier to just have a list of each
    public List<factory> enemy_factories;
    protected int spawn_factory_amount = 3; // There should be 1 factory per 1 spawn


    public int spawn_const_amount { get; } = 2; // When spawning, the amount of enemies is based on spawn_const_amount + (spawn_dice_amount)d4
    public int spawn_dice_amount { get; } = 1;

    public override string game_state_name { get; } = "Enemy_manager";


    void get_order()
    {
        List<sub_state> order = man.ran_man.randomize_list(sstate_bag);

        // Add the scoring every 5 - 7 turns
        int scoring_turn = man.ran_man.rnd.Next(5, 8);

        order.Insert(scoring_turn, man.sub_states["scoring"]);

        sstate_order = new Queue<sub_state>(order);

    }

    /*
     * Get the next state and remove it from sstate_order
     */
    public string get_next()
    {
        return sstate_order.Dequeue().sub_state_name;
    }

    public override void start_state()
    {
        
    }

    public override void end_state()
    {
        
    }

    public override void init()
    {
        //Debug.Log(string.Join(",", man.prefabs.Keys));
        //*
        for (int i = 0; i < spawn_factory_amount; i++)
        {
            GameObject s_go = Instantiate(man.prefabs["spawn"], new Vector3(0, 0, 0), Quaternion.identity);
            spawn s = s_go.GetComponent<spawn>();
            s.init(man.request_id(), man, this);
            enemy_spawns.Add(s);

            GameObject f_go = Instantiate(man.prefabs["factory"], new Vector3(0, 0, 0), Quaternion.identity);
            factory f = f_go.GetComponent<factory>();
            f.init(man.request_id(), man, this);
            enemy_factories.Add(f.GetComponent<factory>());
        }
        //*/
    }

    /*
     * add enemy to enemies list
     */
    public void add_enemy(Enemy e)
    {
        enemies.Add(e);
    }
}
