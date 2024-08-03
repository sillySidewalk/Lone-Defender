using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Pawn
{
    public int atk_value = 10; // How many dice you roll while attacking
    public int atk_distance = 1; // How far away they can attack
    public List<int> atk_mod = new List<int>();  // List of modifiers to player's attack dice, applies to all dice
    public override move_type m_type { get; } = move_type.clear_for;

    public override Location.move_position move_pos { get; } = Location.move_position.player;

    // Add the player pawns to the Location player pawn list, then do the default
    public override void move(Location new_loc)
    {
        current_location.player_pawns.Remove(this);
        move_position(new_loc);
    }

    public int get_atk_sum()
    {
        return atk_mod.Sum();
    }

    
    /*
     * Attack a Clearing
     * parameter:
     *      Clearing - which clearing to attack
     */
    public void attack_clearing(Clearing cl)
    {
        List<int> atk_rolls = man.d_roll(atk_value);

        int cl_mod = cl.get_p_atk_sum();

        int p_mod = get_atk_sum();

        for(int i = 0; i < atk_rolls.Count; i++) 
        {
            atk_rolls[i] += cl_mod + p_mod;
        }

        man.player_pawn_attack(atk_rolls, cl);

    }

   
}
