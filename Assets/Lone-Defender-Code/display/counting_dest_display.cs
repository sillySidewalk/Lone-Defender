using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


/*
 * Because P will want to know where the enemies will be going, this will have to display both the amount of units and their destination
 */
public class counting_dest_display : Location_display
{
    [SerializeField] protected Transform sprite_loc; // The location the the game piece will be, since it won't be center
    [SerializeField] protected TextMeshProUGUI count_txt; // How many are at this location
    [SerializeField] protected TextMeshProUGUI destination_txt; // Display where these enemies are headed
    [SerializeField] public int factory_dest = -1; // Which factory they are headed to, -1 means it's unused


    /*
     * Set the location to sprite_loc, look through location's enemy_pawns to get how many enemies heading to this factory
     */
    public override void add_game_piece(Game_piece go)
    {
        Enemy new_e = (Enemy)go;
        new_e.transform.position = sprite_loc.position;


        // If this display was empty, update factory_dest
        if (factory_dest == -1)
        {
            factory_dest = new_e.fact.id;
        }      

        List<Enemy> loc_e_pawns = loc.pawns["enemy"].ConvertAll(x => (Enemy)x);
        int enemy_count = loc_e_pawns.Where(e => e.fact.id == factory_dest).Count();

        count_txt.text = enemy_count.ToString();
        
    }

    public override void remove_game_piece(Game_piece go)
    {
        Debug.LogError("Implement counting_dest_display => remove_game_piece");
        
        Enemy new_e = (Enemy)go;

        // Get number of enemies with this destination
        List<Enemy> loc_e_pawns = loc.pawns["enemy"].ConvertAll(x => (Enemy)x);
        int enemy_count = loc_e_pawns.Where(e => e.fact.id == factory_dest).Count();

        // If empty, set to -1
        if(enemy_count <= 0)
        {
            factory_dest = -1;
        }
    }

}
