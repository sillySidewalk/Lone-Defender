using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class scheme : MonoBehaviour
{
    [SerializeField] protected Manager man;
    [SerializeField] public string scheme_name;
    [SerializeField] protected enemy_scheme es;
    [SerializeField] protected scheme_ui s_ui;
    [SerializeField] protected string hover_text; 

    public virtual void init(enemy_scheme _es)
    {
        man = Manager.get_instance();
        es = _es;
    }

    public abstract void start_scheme();

    public abstract void scheme_update(); // Called each enemy_scheme sub state. Scheme carries out any "each turn" aspects

    protected virtual void end_scheme()
    {
        s_ui.complete_scheme();
    }

    public void add_ui(scheme_ui s)
    {
        s_ui = s;
    }

    public string get_hov_text()
    {
        return hover_text;
    }
}
