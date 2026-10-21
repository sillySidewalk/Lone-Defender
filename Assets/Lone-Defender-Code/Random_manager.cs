using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/*
 * Controls all the random elements to keep a consistant seed
 */
public class Random_manager : MonoBehaviour
{
    [SerializeField] Manager man;
    [SerializeField] results_ui r_ui;
    public System.Random rnd = new System.Random();

    public void init(results_ui ui)
    {
        r_ui = ui;
    }

    /*
     * Roll the dice n times
     */
    public List<int> d10(int dice_count, string context)
    {
        List<int> return_dice = new List<int>();

        for (int i = 0; i < dice_count; i++)
        {
            return_dice.Add(rnd.Next(1, 10 + 1)); // Max is exclusive, so plus 1
        }

        return_dice.Sort();
        return_dice.Reverse();

        display_dice("d10 rolls(" + return_dice.Count + ") '" + context + "': " + return_dice.List_to_string());

        

        return return_dice;

    }

    public List<int> d4(int n, string context)
    {
        List<int> return_dice = new List<int>();

        for (int i = 0; i < n; i++)
        {
            return_dice.Add(rnd.Next(1, 4 + 1)); // Max is exclusive, so plus 1
        }

        display_dice("d4 rolls (" + return_dice.Count + ") '" + context + "':" + return_dice.List_to_string());

        return return_dice;

    }

    //give general access to random numbers
    public int random_num(int min, int max)
    {
        return rnd.Next(min, max + 1); // Max is exclusive, so plus 1
    }


    /*
     * https://code-maze.com/csharp-randomize-list/
     */
    public List<T> randomize_list<T>(List<T> listToShuffle)
    {
        var shuffledList = listToShuffle.OrderBy(_ => rnd.Next()).ToList();
        return shuffledList;
    }


    /*
     * Randomize a list of number for each clearing, shuffle list and return the requested number of numbers
     * 
     * allows multiple random clearings without duplicates
     */
    public List<int> random_clearing_nums(int num_clearings)
    {
        List<int> clearings = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        clearings = randomize_list(clearings);
        return clearings.GetRange(0, num_clearings);
    }


    /*
     * Allows requester to have certain numbers removed, in case those locations are already in use
     */
    public List<int> random_clearing_nums(int num_clearings, List<int> exclusions)
    {
        List<int> clearings = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
        foreach( int exlusion in exclusions)
        {
            clearings.RemoveAt(exlusion);
        }

        clearings = randomize_list(clearings);
        return clearings.GetRange(0, num_clearings);
    }

    protected void display_dice(string output)
    {
        Debug.Log(output);
        r_ui.add_item(output);
    }
}
