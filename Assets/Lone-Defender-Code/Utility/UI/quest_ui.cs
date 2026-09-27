using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class quest_ui : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI quest_name;
    [SerializeField] protected TextMeshProUGUI reduction_text;
    [SerializeField] protected TextMeshProUGUI progress_text;
    [SerializeField] protected UI_formatter ui_form;
    [SerializeField] protected int abrev_name_length = 5;
    [SerializeField] protected int difficulty_number;

    // take the information from a quest and add it to this ui
    public void init(quest q)
    {
        ui_form = Manager.get_instance().ui_form;

        set_name(q.get_q_name());
        reduction_text.text = q.get_success_reduction().ToString();
        progress_text.text = ui_form.convert_to_ratio(0, q.get_difficulty());
        difficulty_number = q.get_difficulty();

        q.add_ui(this);
    }

    // Because we only have so much room, abbreviate it
    public void set_name(string new_name)
    {
        int abrev_length = Mathf.Min(abrev_name_length, new_name.Length);
        quest_name.text = new_name.Substring(0, abrev_length);
    }

    public void update_progress(int new_progress)
    {
        progress_text.text = ui_form.convert_to_ratio(new_progress, difficulty_number);
    }

    public void quest_completed()
    {
        Destroy(gameObject);
    }
}
