using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/*
 * Player can complete quests to gain victory points
 * 
 * P can have X number of quests active at once. At the start of their turn, randomly draw quests from bag until at X. Once completed, quests are put in discard bag. If need more quests, shuffle discard and put back in draw.
 * 
 */
public class quest_system : MonoBehaviour
{
    [SerializeField] protected Manager man;
    [SerializeField] protected List<LD_token> quest_tokens = new List<LD_token>();
    [SerializeField] protected int num_quest_tokens = 3;
    [SerializeField] protected List<quest> quest_draw_bag = new List<quest>();
    [SerializeField] protected List<quest> quest_discard_bag = new List<quest>();
    [SerializeField] protected List<quest> current_quests = new List<quest>();
    [SerializeField] protected int current_q_amnt = 3; // current quest amount
    [SerializeField] protected int vp = 0;
    [SerializeField] protected int winning_vp = 30;
    
    /*
     * Get quests from this gameObject, randomize and add to draw bag. Init all quests
     */
    public void init()
    {
        List<quest> init_quests = new List<quest>(gameObject.GetComponents<quest>());

        quest_draw_bag = new List<quest>(man.ran_man.randomize_list(init_quests));

        foreach(quest q in quest_draw_bag)
        {
            q.init(man);
        }

        init_quest_tokens();

        draw_current_quests();
    }

    protected void init_quest_tokens()
    {
        for(int i = 0; i < num_quest_tokens; i++)
        {
            GameObject go = Instantiate(man.prefabs["quest_token"]);
            quest_token qt = go.GetComponent<quest_token>();

            qt.init(man, "Q" +  i);

            quest_tokens.Add(qt);
        }
    }

    protected void draw_current_quests()
    {
        quest next_quest;
        int new_quest_count = current_q_amnt - current_quests.Count;
        List<int> exclusions = current_quest_locations();

        List<int> random_locations = man.ran_man.random_clearing_nums(12-new_quest_count, exclusions);


        for(int i = current_quests.Count; i < current_q_amnt; i++)
        {
            next_quest = quest_draw_bag.Dequeue();
            Clearing c = man.clearings[random_locations.Dequeue()];
            next_quest.start_quest(c, quest_tokens.Dequeue());
            current_quests.Add(next_quest);
        }

    }

    public void update_vp(int new_vp)
    {
        vp += new_vp;

        vp = Mathf.Max(vp, 0);

        if(vp >= winning_vp)
        {
            Debug.Log("YOU WIN!!");
        }
    }

    public void quest_finished(quest q, LD_token quest_token)
    {
        update_vp(q.vp_value);

        quest_discard_bag.Add(q);
        current_quests.Remove(q);

        quest_tokens.Add(quest_token);
        reset_token(quest_token);
    }

    public List<int> current_quest_locations()
    {
        List<int> return_list = new List<int>();

        foreach(quest q in current_quests)
        {
            Clearing c = (Clearing) q.loc;
            return_list.Add(c.get_id());
        }

        return return_list;
    }

    public quest get_quest_at_clr(Clearing c)
    {
        foreach(quest q in current_quests)
        {
            if((Clearing) q.loc == c)
            {
                return q;
            }
        }

        //if we made it this far, none of the current quests are in the location
        return null;
    }


    public void attempt_quest_clr(Clearing c, List<int> dice_attemps)
    {
        quest q = get_quest_at_clr(c);

        if(q == null)
        {
            Debug.Log("no quests at current location");
            return;
        }
        else
        {
            q.attempt_progress(dice_attemps);
        }
    }

    public void reset_token(LD_token t)
    {
        t.loc.remove_token(t);
        t.gameObject.SetActive(false);
    }

}
