using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] public List<Clearing> clearings;
    [SerializeField] public List<Road> roads;
    [SerializeField] public List<Forest> forests;
    System.Random rnd = new System.Random();
    int dice_value = 10; // The type of dice
    int min_atk_val { get; } = 7; // What value is considered a hit, base d10 dice

    /*
     * Roll the dice
     */
    public int d_roll()
    {
        return rnd.Next(1, dice_value+1); // Max is exclusive, so plus 1
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

        Debug.Log("hits: " + hits);

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
                Debug.Log("e.Count = " + e.Count + " hits = " + hits);
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

    private void Update()
    {
        List<int> atk_list = new List<int>() { 7, 10 };

        if(Input.GetKeyDown("1"))
        {
            player_pawn_attack(atk_list, clearings[0]);
        }
    }

}
