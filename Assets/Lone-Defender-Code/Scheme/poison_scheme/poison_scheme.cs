using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

/*
 * Add a real and fake poison token
 * 
 * The real poison token applies 1 poison each time P enters the location, but doesn't show this to P. At end of turn, P take 1 damage for 2 poison.
 */
public class poison_scheme : scheme
{
    [SerializeField] protected string prefab_token_name = nameof(poison_token);
    [SerializeField] protected GameObject poison_token_obj;
    [SerializeField] protected poison_token p_token;
    [SerializeField] protected GameObject fake_poison_token_obj;
    [SerializeField] protected poison_token fake_p_token;
    [SerializeField] protected List<int> loc_list; // list of clearings numbers for use of randomization
    [SerializeField] protected int player_poison = 0;
    [SerializeField] protected GameObject token_parent;


    public override void init(enemy_scheme _es)
    {
        base.init(_es);
        loc_list = Enumerable.Range(0, man.clearings.Count).ToList();
        
    }

    protected void poison_obj_init()
    {
        poison_token_obj = GameObject.Instantiate(man.prefabs[prefab_token_name]);
        p_token = poison_token_obj.GetComponent<poison_token>();
        string token_name_number = man.ran_man.random_num(0, 1).ToString();

        p_token.init(this, token_name_number, true);
        p_token.transform.SetParent(man.transform.Find("Scheme_object"), true);
        poison_token_obj.SetActive(false);
    }

    protected void fake_poison_obj_init()
    {
        fake_poison_token_obj = GameObject.Instantiate(man.prefabs[prefab_token_name]);
        fake_p_token = fake_poison_token_obj.GetComponent<poison_token>();

        // choose the opposite of the real token
        string token_name_number = "0";
        if(p_token.get_name_num() == "0")
        {
            token_name_number = "1";
        }
        fake_p_token.init(this, token_name_number, false);
        fake_p_token.transform.SetParent(man.transform.Find("Scheme_object"), true);
        fake_poison_token_obj.SetActive(false);
    }

    public override void start_scheme()
    {
        poison_obj_init();
        fake_poison_obj_init();

        reset_poison();

        List<int> copy_list = new List<int>(loc_list);

        List<int> random_list = man.ran_man.randomize_list(copy_list);

        add_poison_token(man.clearings[random_list[0]]);

        add_fake_token(man.clearings[random_list[1]]);

    }

    protected void add_poison_token(Clearing cl)
    {
        poison_token_obj.SetActive(true);
        p_token.start_token(cl);
    }

    protected void add_fake_token(Clearing cl)
    {
        fake_poison_token_obj.SetActive(true);
        fake_p_token.start_token(cl);
    }

    // When P enters the poison location
    public void add_poison()
    {
        player_poison++;
    }

    protected void reset_poison()
    {
        player_poison = 0;
    }

    protected void apply_damage()
    {
        man.player.adjust_health(-1 * player_poison/2);
    }

    public override void scheme_update()
    {
        apply_damage();
    }

    public void real_investigated()
    {
        Debug.Log("Real posion");

        end_scheme();
        p_token = null;
        poison_token_obj = null;
    }

    public void fake_investigated()
    {
        Debug.Log("Fake posion");
        fake_p_token.end_token();
        fake_p_token = null;
        fake_poison_token_obj = null;
    }

    /*
     * Remove both tokens, reset poison, tell enemy_scheme that scheme is finished
     */
    protected void end_scheme()
    {
        p_token.end_token();
        if(fake_p_token != null)
        {
            fake_p_token.end_token();
        }
        
        reset_poison();

        es.end_scheme(this);
    }
}
