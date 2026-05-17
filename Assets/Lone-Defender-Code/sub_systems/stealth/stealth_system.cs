using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class stealth_system : MonoBehaviour
{
    [SerializeField] protected Manager man;
    [SerializeField] protected Player p;
    [SerializeField] protected int current_stealth;
    [SerializeField] protected int max_stealth;
    [SerializeField] protected int atk_stealth_penalty = 1; // for each attack, reduce current_stealth by this
    [SerializeField] protected int sth_atk_reduct = 3; // for each point of stealth, reduce atk dice by this
    [SerializeField] protected UI_formatter ui_form;
    [SerializeField] protected TextMeshProUGUI text_numbers;

    


    /*
     * initialize system with stats from character type.
     */
    public void init()
    {
        man = Manager.get_instance();
        ui_form = man.ui_form;
        current_stealth = p.starting_stealth;
        max_stealth = p.max_stealth;

        p.stealth_sys = this;

        update_ui();
    }

    public void attack_update_stealth()
    {
        update_stealth(-1 * atk_stealth_penalty);
    }


    protected void update_stealth(int value)
    {
        current_stealth += value;

        current_stealth = Mathf.Clamp(current_stealth, 0, max_stealth);
        update_ui();
    }

    protected void update_ui()
    {
        text_numbers.text = ui_form.convert_to_ratio(current_stealth, max_stealth);
    }

    // Reduce retaliation dice by stealth value
    public int retal_reduce_val()
    {

        return current_stealth * sth_atk_reduct;
    }

    public int atk_reduce_val()
    {
        return current_stealth * sth_atk_reduct;
    }
}
