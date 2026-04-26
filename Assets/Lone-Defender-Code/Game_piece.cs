using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Any object that would be a piece on the board
 */
public abstract class Game_piece : MonoBehaviour
{
    [SerializeField] protected Manager man;
    [SerializeField] protected string ld_name; // The name of Location_display, get prefab from Manager
    //[SerializeField] protected GameObject ld_prefab;
    [SerializeField] public virtual string display_type => "Game_piece";

    public virtual Dictionary<string, List<int>> display_position { get; } // String is the location type, List<int> is the positions it uses on the location type
    

    public virtual void init()
    {
        man = Manager.get_instance();
    }

    public void add_manager(Manager m)
    {
        man = m;
    }
}
