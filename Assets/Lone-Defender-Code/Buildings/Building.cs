using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

/*
 * Damage:
 *      When a building is successfully attacked, it takes hits. When it has a certain amount of hits, the building will take a damage. Each damage will impair the buildings function, depending on the actual building. 
 *      
 *      The amount of hits necessary to increase damage is based on the damage_threshold list, with each position indicating how many hits to increase damage to that level. Eg. to go from 0 to 1 damage is the 0th item in damage_threshold.
 *      
 *      If a building has reached it's max damage threshold, it can't take any more damage and all further hits are ignored.
 *      
 *      When building is repaired, all damage and hits are removed
 *      
 *      
 */
public abstract class Building : Game_piece
{
    [SerializeField] protected int _id;
    [SerializeField] public int id { get { return _id; } }
    [SerializeField] public Location loc; // The location of the buildings
    [SerializeField] public override string display_type => "building";
    //[SerializeField] protected Manager man;
    [SerializeField] protected Enemy_manager e_man;
    [SerializeField] protected int hits;
    [SerializeField] protected int damage;
    [SerializeField] protected List<int> damage_threshold;


    public void init(int init_id, Manager init_man, Enemy_manager init_e_man)
    {
        _id = init_id;
        man = init_man;
        e_man = init_e_man;

        init_damage_threshold();
    }

    public int get_damage()
    {
        return damage;
    }

    // Set the damage threshold for each level of damage
    public abstract void init_damage_threshold();

    public void move(Location new_loc)
    {
        if (loc != null)
        {
            remove_loc();
        }
        loc = new_loc;
        new_loc.add_building(this);
    }

    // Remove self from the loc, if there is one
    public void remove_loc()
    {
        if(loc != null)
        {
            loc.remove_building(this);
        }
    }

    public void apply_hits(int incoming_hits)
    {
        hits += incoming_hits;

        // check if we have a damage threshold
        int next_threshold = get_next_threshold();

        if(next_threshold == -1)
        {
            // At max damage
            return;
        }
        else if(hits >= next_threshold)
        {
            hits = 0;
            damage += 1;
        }

    }

    // returns how many hits to reach next threshold (assuming hits are at 0). If -1 is returned, we're at the last threshold
    protected int get_next_threshold()
    {
        // If we're at the last threshold, don't try to access a non-existant position in array.
        if(damage >= damage_threshold.Count)
        {
            return -1;
        }

        return damage_threshold[damage];
    }

    // Removes all damage and hits
    public void repair()
    {
        hits = 0;
        damage = 0;
    }
}
