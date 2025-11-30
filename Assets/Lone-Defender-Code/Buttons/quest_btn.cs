using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quest_btn : MonoBehaviour
{
    [SerializeField] protected Manager man;

    public void call_quest()
    {
        man.call_quest();
    }
}
