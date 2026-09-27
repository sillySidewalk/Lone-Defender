using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class vp_obj : MonoBehaviour
{
    [SerializeField] public string type;
    [SerializeField] protected TextMeshProUGUI number_text;

    public void set_text(int vp)
    {
        number_text.text = vp.ToString();
    }
}
