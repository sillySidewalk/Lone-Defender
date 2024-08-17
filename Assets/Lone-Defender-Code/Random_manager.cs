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
    public System.Random rnd = new System.Random();

    /*
     * Roll the dice n times
     */
    public List<int> d10(int n)
    {
        List<int> return_dice = new List<int>();

        for (int i = 0; i < n; i++)
        {
            return_dice.Add(rnd.Next(1, 10 + 1)); // Max is exclusive, so plus 1
        }

        return return_dice;

    }

    public List<int> d4(int n)
    {
        List<int> return_dice = new List<int>();

        for (int i = 0; i < n; i++)
        {
            return_dice.Add(rnd.Next(1, 4 + 1)); // Max is exclusive, so plus 1
        }

        return return_dice;

    }


    /*
     * https://code-maze.com/csharp-randomize-list/
     */
    public List<T> randomize_list<T>(List<T> listToShuffle)
    {
        var shuffledList = listToShuffle.OrderBy(_ => rnd.Next()).ToList();
        return shuffledList;
    }
}
