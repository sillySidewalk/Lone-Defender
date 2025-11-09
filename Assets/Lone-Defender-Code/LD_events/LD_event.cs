using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * An individual event
 */
public abstract class LD_event : MonoBehaviour
{
    [SerializeField] protected Manager man;

    protected List<int> loc_list = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

    // by default, nothing, but can be added
    public virtual void init()
    {

    }

    public abstract void start_event();

    public abstract void end_event();

    public List<LD_token> add_token_to_locs(int min_locs, int max_locs, string token_name)
    {
        List<LD_token> token_objs = new List<LD_token>();

        int num_locations = man.ran_man.random_num(min_locs, max_locs);

        List<int> rand_loc_list = man.ran_man.randomize_list<int>(loc_list);

        List<int> new_token_locs = rand_loc_list.GetRange(0, num_locations);

        Location l;

        foreach (int loc in new_token_locs)
        {
            GameObject new_obj = Instantiate(man.prefabs[token_name]);
            LD_token t = new_obj.GetComponent<LD_token>();
            t.add_manager(man);
            token_objs.Add(t);

            l = man.clearings[loc];

            l.add_token(t, "event");
        }

        return token_objs;
    }
}
