using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


/*
 * Enemy will perform special actions.
 * 
 * Examples:
 *      setting traps
 *      
 */
public class enemy_scheme : auto_exit_sub_state
{
    public override string called_next_state => throw new System.NotImplementedException();

    public override string sub_state_name => "enemy_scheme";

    public override bool loc_click_sub => false;

    [SerializeField] List<scheme> scheme_list;
    [SerializeField] List<scheme> inactive_schemes;
    [SerializeField] List<scheme> active_schemes;

    public override void end_state()
    {
        
    }

    public override void init()
    {
        man = Manager.get_instance();
        fetch_schemes();
        init_schemes();
    }

    public override void loc_click(Location loc)
    {
        throw new System.NotImplementedException();
    }

    /*
     * Let each scheme know a turn has passed, then start a new scheme
     */
    protected override void sub_state_work()
    {
        foreach(scheme s in active_schemes)
        {
            s.scheme_update();
        }

        if(inactive_schemes.Count <= 0) // can't start a new scheme if we don't have any not active
        {
            Debug.Log("no schemes, pass");
            return;
        }

        int random_scheme = man.ran_man.random_num(0, inactive_schemes.Count - 1);

        scheme new_scheme = inactive_schemes[random_scheme];

        new_scheme.start_scheme();

        active_schemes.Add(new_scheme);
        inactive_schemes.Remove(new_scheme);

    }

    protected void init_schemes()
    {
        foreach(scheme s in scheme_list)
        {
            s.init(this);
        }

        inactive_schemes = new(scheme_list);
    }

    protected void fetch_schemes()
    {
        scheme_list = man.GetComponentsInChildren<scheme>().ToList();
    }

    public void end_scheme(scheme s)
    {
        inactive_schemes.Add(s);
        active_schemes.Remove(s);
    }
}
