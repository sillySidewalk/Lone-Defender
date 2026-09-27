using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class state_btn : MonoBehaviour
{
    [SerializeField] Manager man;
    [SerializeField] string state_name;

    public void init()
    {
        man = Manager.get_instance();

        
        if(!man.states.ContainsKey(state_name))
        {
            Debug.LogError("state_btn: " + state_name + " doesn't match any states");
        }
    }

    public void Call_state()
    {
        man.call_state(state_name);
    }
}
