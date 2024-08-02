using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_btn : MonoBehaviour
{
    [SerializeField] Manager man;

    public void OnButtonPress()
    {
        man.call_sub_state("player_move");
    }
}
