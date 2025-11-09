using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightning_token : LD_token
{
    public override string token_name => "lightning_token";

    public override void add_effect()
    {
        Debug.Log("Adding lightning effect");
    }

    // attack 3 times
    public override void remove_effect()
    {
        List<int> atks = man.ran_man.d10(3);
        Debug.Log(_loc + man.list_to_string(atks));
        man.attack_enemy(atks, (Clearing)_loc);
        man.attack_player(atks, (Clearing)_loc);




    }
}
