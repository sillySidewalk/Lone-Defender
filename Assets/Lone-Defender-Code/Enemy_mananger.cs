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
public class Enemy_mananger : game_state
{
    List<sub_state> sub_states; // The possible turn types
    List<sub_state> sstate_bag; // The bag to be drawn from, can have multiple occurances of a given sub_state
    Queue<sub_state> sstate_order; // The order that turn types will occur
    System.Random rnd = new System.Random();

    public override string game_state_name { get; } = "Enemy_mananger";


    /*
     * https://code-maze.com/csharp-randomize-list/
     */
    public List<T> randomize_list<T>(List<T> listToShuffle)
    {
        var shuffledList = listToShuffle.OrderBy(_ => rnd.Next()).ToList();
        return shuffledList;
    }

    void get_order()
    {
        List<sub_state> order = randomize_list(sstate_bag);

        // Add the scoring every 5 - 7 turns
        int scoring_turn = rnd.Next(5, 8);

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
        throw new NotImplementedException();
    }

    public override void end_state()
    {
        throw new NotImplementedException();
    }
}
