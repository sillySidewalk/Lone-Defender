using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*
 * Both the real and fake poison token
 */
public class poison_token : scheme_token
{
    public override string token_name => "poison_token";

    //[SerializeField] protected poison_scheme ps;

    [SerializeField] protected TextMeshPro name_txt; // 1 or 2 

    [SerializeField] protected bool is_real; // whether the real or fake

    [SerializeField] protected Clearing current_loc;


    public void init(poison_scheme _ps, string _name, bool _is_real)
    {
        parent_scheme = _ps;
        name_txt.text = _name;
        is_real = _is_real;

        man = Manager.get_instance();
    }

    /*
     * Move the token and add observer to location
     */
    public void start_token(Clearing c)
    {
        c.add_token(this);
        current_loc = c;

        if(is_real)
        {
            ((player_move)man.sub_states["player_move"]).movement_e += check_poison_add;
        }
    }

    public void end_token()
    {
        if(is_real)
        {
            ((player_move)man.sub_states["player_move"]).movement_e -= check_poison_add;
        }

        loc.remove_display_highlights(display_type);
        loc.remove_token(this);
    }

    public string get_name_num()
    {
        return name_txt.text;
    }

    /*
     * If P moved to this location, add poison
     * 
     * handler for subscription
     */
    protected void check_poison_add(object sender, p_move_event_args e)
    {
        if(e.end_move == current_loc)
        {
            ((poison_scheme)parent_scheme).add_poison();
        }
    }

    public override void investigate()
    {
        if(is_real)
        {
            ((poison_scheme)parent_scheme).real_investigated();
        }
        else
        {
            ((poison_scheme)parent_scheme).fake_investigated();
        }
    }
}
