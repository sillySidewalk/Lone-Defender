using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Quests will have a location that P will have to be at to complete
 * 
 * Quests have a difficulty, which is how many successful attacks it requires to complete. successes will be tracked between attempts. Some can have reduction, meaning that each attempt will have the number of successful attacks reduced by X, so P will need at least X+1 successful attacks to get a success on the quest
 */
public class quest : MonoBehaviour
{
    [SerializeField] protected Manager man;
    [SerializeField] protected quest_system qs;
    [SerializeField] protected LD_token quest_token;
    [SerializeField] protected string q_name;
    [SerializeField] public Location loc;
    [SerializeField] public int vp_value = 0;
    [SerializeField] protected int difficulty = 0;
    [SerializeField] protected int success_reduction = 0;
    [SerializeField] protected int progress = 0; // how many successes they have

    public void init(Manager _man)
    {
        man = _man;
        qs = _man.qs;
        quest_token = _man.prefabs["quest_token"].GetComponent<quest_token>();
    }

    // Add quest to map and any other setup necessary after init
    public void start_quest(Clearing c, LD_token t)
    {
        quest_token = t;
        quest_token.gameObject.SetActive(true);
        loc = c;

        c.add_token(quest_token);
    }

    // player try for successess on quest
    public void attempt_progress(List<int> attempts)
    {
        bool check_deduct_val = man.player.ap_system.check_deduct_ap(1);
        if (!check_deduct_val)
        {
            Debug.Log("Not enough actions points");
            return;
        }

        int successes = 0;

        foreach(int attempt in attempts)
        {
            if(attempt >= man.min_atk_val)
            {
                successes++;
            }
        }

        successes -= success_reduction;
        successes = Mathf.Max(0, successes);
        progress += successes;


        if(progress >= difficulty)
        {
            complete_quest();
        }
    }

    public void complete_quest()
    {
        Debug.Log("Finished quest: " + q_name + " + " + vp_value + " vp");
        qs.quest_finished(this, quest_token);
        reset_quest();
    }

    protected void reset_quest()
    {
        progress = 0;
        quest_token = null;
        loc = null;
    }
}
