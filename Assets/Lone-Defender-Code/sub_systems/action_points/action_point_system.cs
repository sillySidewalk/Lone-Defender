using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class action_point_system: MonoBehaviour
{
    [SerializeField] protected int action_point_max;
    [SerializeField] protected int action_p_per_round; // how many player gets at the start of the round
    [SerializeField] protected int action_points_current;
    public TextMeshProUGUI action_points_ui;
    //protected List<action> available_actions;

    public void init(int _ap_max, int _ap_turn, TextMeshProUGUI ap_ui)
    {
        action_points_ui = ap_ui;
        action_p_per_round = _ap_turn;
        action_point_max = _ap_max;
        action_points_current = 0;
        update_ui();
        //init_actions();
    }

    /* Probably not necessary
    public void init_actions()
    {
        
        foreach(action a in  available_actions)
        {
            a.init();
        }
        
    }
    */

public void update_ui()
    {
        string ui_string = action_points_current.ToString() + "/" + action_point_max.ToString();
        action_points_ui.text = ui_string;
    }

    public int get_action_points()
    {
        return action_points_current;
    }

    /*
     * Relative to current amount. Also updates UI.
     * 
     * Check if this would reduce to zero or below and returns -1, otherwise 1; Clamps actions points between 0 and action_points_max
     */
    public int subtract_action_points(int x)
    {
        int ret_val = 0;
        int new_val = action_points_current - x;

        if(new_val < 0)
        {
            ret_val = -1;
        }


        action_points_current = Mathf.Clamp(new_val, 0, action_point_max);
        update_ui();
        return ret_val;
    }

    /*
     * Relative to current amount. Also updates UI.
     * 
     * Check if this would reduce to max or more and returns -1, otherwise 0; Clamps actions points between 0 and action_points_max
     */
    public int add_action_points(int x)
    {
        int ret_val = 0;

        int new_val = action_points_current + x;
        
        if (new_val >= action_point_max)
        {
            ret_val = -1;
        }

        action_points_current = Mathf.Clamp(new_val, 0, action_point_max);
        update_ui();
        return ret_val;
    }


    /*
     *  Checks if there are enough ap to pay for action. if there is, then subtracts them. Otherwise, ap is left alone.
     *  
     *  Returns false if not enough and ap not changed, returns true if enough and sutracts ap
    */
    public bool check_deduct_ap(int ap_cost)
    {
        int diff = action_points_current - ap_cost;

        if (diff >= 0)
        {
            subtract_action_points(ap_cost);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void refill_action_point()
    {
        add_action_points(action_p_per_round);
    }
}
