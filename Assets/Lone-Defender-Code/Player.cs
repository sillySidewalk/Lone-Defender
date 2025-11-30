using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Player : Pawn
{
    public int atk_value = 10; // How many dice you roll while attacking
    public int atk_distance = 0; // How far away they can attack
    public List<int> atk_mod = new List<int>();  // List of modifiers to player's attack dice, applies to all dice
    public List<int> def_mod = new List<int>(); // List of modifiers to received attacks, aplies to all dice
    public action_point_system ap_system;
    public stealth_system stealth_sys;
    protected int action_point_max = 12;
    protected int action_points_per_round = 6;
    [SerializeField] protected int max_hp;
    [SerializeField] protected int hp;
    [SerializeField] public int max_stealth = 10;
    [SerializeField] public int starting_stealth = 5;

    


    public override move_type m_type { get; } = move_type.clear_for;

    public override Location.move_position move_pos { get; } = Location.move_position.player;

    

    public void init(int given_id, Location _loc, TextMeshProUGUI ap_ui)
    {
        base.init(given_id, _loc);
        hp = max_hp;
        init_action_point_system(action_point_max, action_points_per_round ,ap_ui);
        stealth_sys.init();
    }

    public void init_action_point_system(int _max_ap, int _ap_turn, TextMeshProUGUI _ap_ui)
    {
        ap_system.init(_max_ap, _ap_turn, _ap_ui);
    }



    // Add the player pawns to the Location player pawn list, then do the default
    public override void move(Location new_loc)
    {
        //current_location.player_pawns.Remove(this);
        base.move(new_loc);
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
        List<int> atk_rolls = man.ran_man.d10(atk_value);

        int cl_mod = cl.get_p_atk_sum();

        int p_mod = get_atk_sum();

        for(int i = 0; i < atk_rolls.Count; i++) 
        {
            atk_rolls[i] += cl_mod + p_mod;
        }

        man.attack_enemy(atk_rolls, cl);


        // retaliation happens before stealth reduction
        man.enemy_man.retal_system.attack_retaliation();

        stealth_sys.attack_update_stealth();

    }

    /* 
     * Apply defense mods, then subtract health for each hit
     * 
     * because we modify the list, we make a copy so the original is left alone.
     */
    public void receive_atk(List<int> in_atks)
    {
        int def_sum = def_mod.Sum();
        List<int> atks = new List<int>(in_atks);

        for(int i = 0;i < atks.Count;i++)
        {
            atks[i] -= def_sum;
        }

        foreach(int atk in atks)
        {
            if(atk >= man.min_atk_val)
            {
                adjust_health(-1);
            }
        }
    }



    /*
     * Relative change to health, adding or subtracting
     */
    public void adjust_health(int value)
    {
        hp += value;
        hp = Mathf.Clamp(hp, 0, max_hp);
    }

    public List<int> quest_attempts()
    {
        return man.ran_man.d10(atk_value);
    }

}
