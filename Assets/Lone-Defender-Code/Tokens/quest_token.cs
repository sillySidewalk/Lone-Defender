using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class quest_token : LD_token
{
    [SerializeField] public override string display_type => "quest_token";
    public override string token_name => "quest_token";

    public TextMeshPro name_txt;

    public void init(Manager _man, string name)
    {
        man = _man;
        name_txt.text = name;
    }

}
