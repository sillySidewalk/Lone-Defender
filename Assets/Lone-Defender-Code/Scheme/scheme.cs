using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class scheme : MonoBehaviour
{
    [SerializeField] protected Manager man;
    public string scheme_name;
    protected enemy_scheme es;

    public virtual void init(enemy_scheme _es)
    {
        man = Manager.get_instance();
        es = _es;
    }

    public abstract void start_scheme();

    public abstract void scheme_update(); // Called each enemy_scheme sub state. Scheme carries out any "each turn" aspects
}
