using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;



public class Clearing : Location
{

    [SerializeField] List<int> en_def_mods; // List of the Enemies Defense Modifiers, applies to each attack from player
    [SerializeField] List<int> p_atk_mods; // List of Player attack Modifiers for this location, applies to each attack from player
    protected Dictionary<int, GameObject> id_to_arrow = new();
    [SerializeField] List<GameObject> arrow = new(); // For setting up id_to_arrow
    [SerializeField] List<int> arrow_to = new(); // For setting up id_to_arrow, the clearing id the arrow points to
    //[SerializeField] protected TextMeshProUGUI corruption_token_count_txt;
    /* This doesn't address the issue of tokens like corruption having their own counter
    protected List<string> token_location_name = new();
    protected List<GameObject> token_location_pos;
    protected Dictionary<string, GameObject> token_locations = new();
    */
    public override string location_type { get; } = "Clearing";


    public override void init()
    {
        base.init();

        init_arrow_dictionary();
        init_token_dictionary();
        //init_token_location_dictionary();
    }

    protected void init_arrow_dictionary()
    {
        for (int i = 0; i < arrow_to.Count; i++)
        {
            id_to_arrow.Add(arrow_to[i], arrow[i]);
        }
    }

    /*
     * Add token types specific to clearings to the location dictionary
     */
    protected void init_token_dictionary()
    {
        /* old, not needed
         * Don't overwrite the list at this string if it already exists
        if(!tokens.ContainsKey("corruption token"))
        {
            tokens["corruption token"] = new List<LD_token>();
        }
        */
    }

    /*
    protected void init_token_location_dictionary()
    {
        if (token_location_name.Count != token_location_pos.Count)
        {
            Debug.LogError("Clearing init_token_location_dictionary: token_location_name.Count must equal token_location_pos.Count");
            return;
        }

        for(int i = 0; i < token_location_name.Count; i++)
        {

        }
    }
    //*/

    /*
     * Return the sum of the Enemies Defense Modifiers
     */
    public int get_en_def_sum()
    {
        return en_def_mods.Sum();
    }

    public int get_p_atk_sum()
    {
        return p_atk_mods.Sum();
    }

    
    

    public void activate_arrow(bool to_activate, List<Clearing> destination_clearings)
    {
        foreach (Clearing c in destination_clearings)
        {
            id_to_arrow[c.get_id()].SetActive(to_activate);
        }
    }


    
    public void add_corruption(corruption_token ct)
    {
        Debug.LogError("Rewrite add_corruption in Clearing.cs");
        List<LD_token> corruption_tokens = tokens[ct.token_name];
        corruption_tokens.Add(ct);
        ct.move(this);
        //corruption_token_count_txt.text = corruption_tokens.Count.ToString();
    }

    public void print_pawns()
    {
        Debug.Log(pawns.Keys.Count);
        foreach(string k in pawns.Keys)
        {
            Debug.Log(pawns.Keys.ToString());
        }
    }
    
    /*
    public void add_token(LD_token t)
    {
        tokens[t.token_name].Add(t);

    }
    */
}
