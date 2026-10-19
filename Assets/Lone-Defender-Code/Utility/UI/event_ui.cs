using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class event_ui : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI event_name;
    [SerializeField] protected int abrev_name_length = 8;
    [SerializeField] protected hover_text_ui hov_text;

    public void init(LD_event e)
    {
        set_name(e.event_name);
        e.add_ui(this);
        give_text_to_hover(e);
    }

    // Because we only have so much room, abbreviate it
    protected void set_name(string new_name)
    {
        int abrev_length = Mathf.Min(abrev_name_length, new_name.Length);
        event_name.text = new_name.Substring(0, abrev_length);
    }

    protected void give_text_to_hover(LD_event e)
    {
        Debug.Log(e.get_hov_text());
        hov_text.add_text(e.get_hov_text());
    }

    public void end_event_ui()
    {
        Destroy(gameObject);
    }
}
