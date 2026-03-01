using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class retaliation_system : MonoBehaviour
{
    [SerializeField] protected Manager man;
    [SerializeField] protected Enemy_manager e_man;
    [SerializeField] protected Player p;

    public void init()
    {
        p = man.player;
    }


    /*
     * After Player attacks, enemies retaliate
     * 
     * Retaliation dice is based off number of enemies * enemy_managers attack dice, then reduced by stealth, then rolled and applied to P
     */
    public void attack_retaliation()
    {
        
        Clearing player_clearing = (Clearing) p.current_location;

        int retal_dice_count = player_clearing.get_count("Enemy") * e_man.enemy_atk_dice;


        retal_dice_count -= p.stealth_sys.retal_reduce_val();

        List<int> retal_rolls = man.ran_man.d10(retal_dice_count, "attack_retaliation");

        man.attack_player(retal_rolls, player_clearing);
    }
}
