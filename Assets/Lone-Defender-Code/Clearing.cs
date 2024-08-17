using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;



public class Clearing : Location
{

    [SerializeField] List<int> en_def_mods; // List of the Enemies Defense Modifiers, applies to each attack from player
    [SerializeField] List<int> p_atk_mods; // List of Player attack Modifiers for this location, applies to each attack from player
    

    /*
     * Return the sum of the Enemies Defense Modifiers
     */
    public int get_en_def_sum()
    {
        return en_def_mods.Sum();
    }

    public int get_p_atk_sum()
    {
        return p_atk_mods.Sum();
    }

    /*
     * Check the pawns move position to see if it's an enemy and should update the text
     */
    public void update_enemy_cnt_text(Pawn p)
    {
        if (p.move_pos == Location.move_position.enemy)
        {
            enemy_count_txt.text = enemy_pawns.Count.ToString();
        }
    }

    /*
     * If it's an enemy, increase the enemy text
     * 
     * then do normal move stuff
     */
    public override void add_pawn(Pawn p)
    {
        base.add_pawn(p);

        update_enemy_cnt_text(p);
    }

    public override void remove_pawn(Pawn p)
    {

        base.remove_pawn(p);

        update_enemy_cnt_text(p);
    }
}
