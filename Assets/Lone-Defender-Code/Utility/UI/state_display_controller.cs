using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*
 * Perform functions on the Enemy_state_display_square
 */
public class state_display_controller : MonoBehaviour
{
    [SerializeField] protected Manager man;
    [SerializeField] protected List<state_display_obj> displays;

    public void init()
    {
        man = Manager.get_instance();

        displays = new(GetComponentsInChildren<state_display_obj>());

        foreach(state_display_obj d in displays)
        {
            d.init();
        }

        man.states[nameof(enemy_end_turn)].on_end_state += update_highlights;
    }

    public void fill_state_display(List<string> states)
    {
        clear_state_display();

        foreach (string state in states)
        {
            add_state_item(state);
        }
    }


    protected void add_state_item(string state_name)
    {
        GameObject display_prefab = man.prefabs["enemy_state_display_square"];
        GameObject instance = Instantiate(display_prefab);
        instance.transform.SetParent(transform, false);

        displays.Add(instance.GetComponent<state_display_obj>());

        instance.GetComponentInChildren<TextMeshProUGUI>().text = man.ui_form.enemy_state_abrev(state_name);

    }

    protected void clear_state_display()
    {
        foreach (state_display_obj display in displays)
        {
            Destroy(display.gameObject);
        }

        displays.Clear();
    }


    /*
     * Highlight state displays from 0 to index
     * 
     * if index is -1, remove all highlight
     */
    public void highlight(int index)
    {
        
        if(index == -1)
        {
            remove_highlights();
            return;
        }
        
        for(int i = 0; i < displays.Count && i < index; i++)
        {
            displays[i].set_highlight(true);
        }
    }

    protected void remove_highlights()
    {
        foreach(state_display_obj d in displays)
        {
            d.set_highlight(false);
        }
    }

    protected void update_highlights(object sender, EventArgs args)
    {
        int count = displays.Count - man.enemy_man.get_state_count();

        highlight(count);
    }
}
