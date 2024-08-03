using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sub_state_btn : MonoBehaviour
{
    [SerializeField] Manager man;
    [SerializeField] string sub_state_name;

    public void Call_sub_state()
    {
        man.call_sub_state(sub_state_name);
    }
}
