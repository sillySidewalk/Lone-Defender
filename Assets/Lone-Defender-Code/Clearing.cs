using AYellowpaper.SerializedCollections;
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
    [SerializeField] protected SerializedDictionary<int, GameObject> id_to_arrow = new();
    [SerializeField] List<GameObject> arrow = new(); // For setting up id_to_arrow
    [SerializeField] List<int> arrow_to = new(); // For setting up id_to_arrow, the clearing id the arrow points to
    //[SerializeField] protected TextMeshProUGUI corruption_token_count_txt;
    public override string location_type { get; } = "Clearing";


    public override void init()
    {
        base.init();

        init_arrow_dictionary();
    }

    protected void init_arrow_dictionary()
    {
        for (int i = 0; i < arrow_to.Count; i++)
        {
            id_to_arrow.Add(arrow_to[i], arrow[i]);
        }
    }

    

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
    
    
}
