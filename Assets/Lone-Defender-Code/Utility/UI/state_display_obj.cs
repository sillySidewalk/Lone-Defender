using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class state_display_obj : MonoBehaviour
{
    [SerializeField] GameObject highlight_obj;

    public void init()
    {
        highlight_obj = transform.Find("highlight").gameObject;
    }


    public void set_highlight(bool enable)
    {
        highlight_obj.SetActive(enable);
    }
}
