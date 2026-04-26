using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*
 * 
 * 
 * Currently only scheme_tokens are investigatable. I had thought that there might be others, which is why I originally made it with other options in mind. Currently, I'm going to just keep it as only checking scheme_tokens. If I decide to make it more, I'll make an Interface for Investigatables
 */
public class player_investigate : sub_state
{

    public override string called_next_state => "player_choose";

    public override string sub_state_name => "player_investigate";

    public override bool loc_click_sub => false;

    [SerializeField] protected TextMeshProUGUI btn_text;
    [SerializeField] List<string> button_texts = new() { "Investigate", "end Inv" };
    //[SerializeField] protected List<string> investigatables = new() { "scheme_token" }; // Currently only scheme tokens are investigatable, but just in case
    [SerializeField] protected bool check_update = false;
    [SerializeField] protected scheme_token current_investigation_st;

    public override void start_state()
    {
        btn_text.text = button_texts[1];
        highlight_interactables();
        check_update = true;
    }

    public override void end_state()
    {
        btn_text.text = button_texts[0];
        remove_interactibles_highlights();
        check_update = false;
    }

    public override void init()
    {
        base.init();
    }

    public override void loc_click(Location loc)
    {
        throw new System.NotImplementedException();
    }

    protected void highlight_interactables()
    {
        Location p_loc = man.player.current_location;
        p_loc.highlight_locations(nameof(scheme_token), man.location_highlight_hex);
        
    }

    protected void remove_interactibles_highlights()
    {
        Location p_loc = man.player.current_location;

        p_loc.remove_display_highlights(nameof(scheme_token));

    }

    protected override void ap_act()
    {
        current_investigation_st.investigate();
        current_investigation_st = null;
    }

    protected void investigate_item(scheme_token st)
    {
        current_investigation_st = st;
        ap_process(1);
    }

    // Check if P directly clicked on the Investigatable or if they clicked on the display
    protected void check_for_investigatables(Collider2D col)
    {
        scheme_token st = col.GetComponent<scheme_token>();

        if (st != null)
        {
            investigate_item(st);
            return;
        }

        Location_display ld = col.GetComponent<Location_display>();

        if (ld != null)
        {
            Game_piece gp = ld.get_game_piece();
            if(gp is scheme_token)
            {
                investigate_item( (scheme_token) gp);
            }
        }
    }

    protected void check_col_results(Collider2D[] results)
    {
        foreach (Collider2D col in results)
        {
            check_for_investigatables(col);
        }
    }


    private void Update()
    {
        if (check_update)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                Collider2D[] results = Physics2D.OverlapPointAll(mouse_pos);

                check_col_results(results);
            }
        }
    }

}
