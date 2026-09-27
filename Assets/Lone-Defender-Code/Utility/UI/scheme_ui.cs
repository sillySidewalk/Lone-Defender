using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class scheme_ui : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI scheme_name;
    [SerializeField] protected int abrev_name_length = 8;
    [SerializeField] int ending_length = "_scheme".Length;
    [SerializeField] hover_text_ui hov_text;

    public void init(scheme s)
    {
        set_name(s.scheme_name);
        s.add_ui(this);
        give_text_to_hover(s);
    }

    // Because we only have so much room, abbreviate it
    protected void set_name(string new_name)
    {
        string without_ending = new_name.Substring(0, new_name.Length - ending_length);

        int abrev_length = Mathf.Min(abrev_name_length, without_ending.Length);
        scheme_name.text = without_ending.Substring(0, abrev_length);
    }

    public void complete_scheme()
    {
        Destroy(gameObject);
    }

    protected void give_text_to_hover(scheme s)
    {
        hov_text.add_text(s.get_hov_text());
    }
}
