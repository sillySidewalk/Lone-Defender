using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Xml;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public List<Clearing> clearings;
    public List<Road> roads;
    public List<Forest> forests;
    [SerializeField] protected GameObject game_state_obj;
    [SerializeField] protected GameObject sub_state_obj;
    public Dictionary<string, game_state> game_states = new();
    public Dictionary<string, sub_state> sub_states = new();
    public sub_state current_sub_state;
    public game_state current_game_state;
    protected List<sub_state> loc_clk_sstage_subscription = new List<sub_state>(); // which substates want to be told about a Location click
    public Player player;
    System.Random rnd = new System.Random();
    int dice_value = 10; // The type of dice
    int min_atk_val { get; } = 8; // What value is considered a hit, base d10 dice

    public enum Gamestate
    {
        player_turn,
    }

    public enum substate
    {
        player_defaut,
        player_move,
    }

    private void Awake()
    {
        init();
    }


    public void init()
    {
        init_sub_states();
        init_game_states();
    }


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


    /*
     * For initializing, get the sub_state game object with all the sub_states, then add them to the list of sub_states
     */
    protected void init_sub_states()
    {
        List<sub_state> sstates = sub_state_obj.GetComponents<sub_state>().ToList();

        foreach(sub_state sstate in sstates)
        {
            sub_states.Add(sstate.sub_state_name, sstate);

            // If they want a location click event
            if(sstate.loc_click_sub)
            {
                loc_clk_sstage_subscription.Add(sstate);
            }
        }

        // set choose_sub_state as initial state and call it's start
        current_sub_state = sub_states["choose_sub_state"];
        current_sub_state.start_state();
    }

    protected void init_game_states()
    {
        List<game_state> gstates = game_state_obj.GetComponents<game_state>().ToList();

        foreach(game_state gstate in gstates)
        {
            game_states.Add(gstate.game_state_name, gstate);
        }

        current_game_state = game_states["player_turn"];
        current_game_state.start_state();
    }


    /*
     * When a Location is clicked, it will call this. Then Manager will check if the current sub state wants to be notified, then send over the information
     */
    public void location_click(Location loc)
    {
        if(loc_clk_sstage_subscription.Contains(current_sub_state))
        {
            current_sub_state.loc_click(loc);
        }
    }

    public void change_sub_state(String sub_state_name)
    {
        current_sub_state.end_state();
        current_sub_state = sub_states[sub_state_name];
        current_sub_state.start_state();
    }

    private void Update()
    {
        if (Input.GetKeyDown("d"))
        {
            Debug.Log("sub_states");
            foreach(KeyValuePair<string, sub_state> entry in sub_states)
            {
                Debug.Log(entry.Key + " = " + entry.Value);
            }

            Debug.Log("game_states");
            foreach (KeyValuePair<string, game_state> entry in game_states)
            {
                Debug.Log(entry.Key + " = " + entry.Value);
            }
        }
    }

}
