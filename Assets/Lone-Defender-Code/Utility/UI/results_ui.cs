using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


/*
 * Manages the results_ui items in content 
 */
public class results_ui : MonoBehaviour
{
    // The Scroll Views Content item, we add out items to this
    [SerializeField] protected GameObject Content;
    [SerializeField] protected GameObject ui_obj;
    [SerializeField] protected List<GameObject> results_list;

    public void init(GameObject prefab)
    {
        ui_obj = prefab;
    }

    public void add_item(string new_item)
    {
        GameObject new_obj = GameObject.Instantiate(ui_obj);
        TextMeshProUGUI text = new_obj.GetComponentInChildren<TextMeshProUGUI>();
        text.text = new_item;

        new_obj.transform.SetParent(Content.transform, false);

        results_list.Add(new_obj);
    }
}
