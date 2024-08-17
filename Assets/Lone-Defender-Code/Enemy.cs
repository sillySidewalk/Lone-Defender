using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : Pawn
{
    protected Enemy_manager e_man;
    public override Location.move_position move_pos { get; } = Location.move_position.enemy;
    public override move_type m_type { get; } = move_type.clearings;
    public factory fact; // The factory they're moving towards

    public override void init(int given_id, Location _loc)
    {
        Debug.LogError("Enemy should not use this init");
        base.init(given_id, _loc);
    }

    public void init(int init_id, Location init_loc, Manager init_man, Enemy_manager init_e_man, factory init_fact)
    {
        fact = init_fact;
        man = init_man;
        e_man = init_e_man;
        e_man.add_enemy(this);
        base.init(init_id, init_loc);
    }

    // Add the enemy pawn to the new locations enemy_pawns list, then do everything else the same
    public override void move(Location new_loc)
    {
        base.move(new_loc);
    }

    /*
     * Move the enemy one clearing closer to their factory
     */
    public void march()
    {
        // If we're already at the factory, do nothing
        if(fact.loc == current_location)
        {
            return;
        }
        List<Location> path = man.find_path(current_location, fact.loc, m_type);

        // Since the 0 element is the start location, get 1 element
        move(path[1]);
    }
}
