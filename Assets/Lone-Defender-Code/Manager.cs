using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] public List<Clearing> clearings;
    [SerializeField] public List<Road> roads;
    [SerializeField] public List<Forest> forests;
    System.Random rnd = new System.Random();
    int dice_value = 10; // The type of dice
    int min_atk_val { get; } = 8; // What value is considered a hit, base d10 dice

    /*
     * Roll the dice n times
     */
    public List<int> d_roll(int n)
    {
        List<int> return_dice = new List<int>();

        for(int i = 0; i < n; i++)
        {
            return_dice.Add(rnd.Next(1, dice_value + 1)); // Max is exclusive, so plus 1
        }
        
        return return_dice;

    }


    /*
     * An attack (from player pawn) to Clearing
     * parameters:
     *      List of ints representing attacks
     *      Clearing they are attacking
     */
    public void player_pawn_attack(List<int> attacks, Clearing cl)
    {
        int def_mod = cl.get_en_def_sum();
        int hits = 0;

        foreach(int atk in attacks)
        {
            if ((atk - def_mod) >= min_atk_val )
            {
                hits++;
            }
        }

        deal_player_hits(hits, cl);
    }


    public void deal_player_hits(int hits, Clearing cl)
    {
        List<Pawn> e = cl.enemy_pawns;
        Pawn p;

        if(e.Count > 0)
        {
            // For each hit, remove an enemy pawn, stop if we run out of hits or run out of Enemy Pawns
            for (; hits > 0 && e.Count > 0; hits--)
            {
                p = e[0];
                e.RemoveAt(0);
                Destroy(p.gameObject);
            }
        }

        if(hits > 0)
        {
            Debug.Log("Remaining " + hits + " hits would go to Buildings or Tokens");
        }
        
    }

    
    /*
     * Move the enemy pawns. They can only move from one clearing to adjacent clearing     
     */
    public void move_enemies(Clearing start_cl, Clearing end_cl, int num)
    {
        List<Pawn> e_pawns = start_cl.enemy_pawns;

        // Check if there's enough enemy pawns
        if( e_pawns.Count < num )
        {
            Debug.LogError("Not enough enemies in start clearing");
            return;
        }

        // Since all enemies are the sane, just move the first num enemies
        List<Pawn> moving_pawns = e_pawns.Take(num).ToList();
        
        e_pawns.RemoveRange(0, num);

        foreach(Pawn p in moving_pawns)
        {
            p.move(end_cl);
        }

        end_cl.enemy_count_txt.text = num.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            
        }
    }

}
