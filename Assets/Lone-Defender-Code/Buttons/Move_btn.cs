using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_btn : MonoBehaviour
{
    [SerializeField] Manager man;

    public void OnButtonPress()
    {
        man.change_sub_state("player_move");
    }
}
