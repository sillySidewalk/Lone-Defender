using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class retaliation_system : MonoBehaviour
{
    Manager man;
    Enemy_manager e_man;
    Player p;

    public void init()
    {
        p = man.player;
    }


    /*
     * After Player attacks, enemies retaliate
     * 
     * Retaliation dice is based off number of enemies * 2, then reduced by stealth, then rolled and applied to P
     */
    public void attack_retaliation()
    {
        Clearing player_clearing = (Clearing) p.current_location;

        int retal_dice = player_clearing.get_enemies().Count * 2;

        retal_dice = p.stealth_sys.reduce_retal_dice(retal_dice);

        List<int> retal_rolls = man.ran_man.d10(retal_dice);

        man.attack_player(retal_rolls, player_clearing);
    }
}
