using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * dict_helper should create a dictionary going from a string to another type, could be one value or a list
 * 
 */
public class dict_helper<T> : MonoBehaviour
{
    [SerializeField] protected List<string> dict_keys = new();
    [SerializeField] protected List<T> dict_values = new();
    protected Dictionary<string, T> dict;

    public void dict_to_str_list()
    {
        create_dict();
    }

    public void init_dict()
    {
        create_dict();
    }

    public void init_dict(Dictionary<string, T> reference)
    {
        create_dict();
        reference = dict;
    }

    /* 
     * Take all the data in dict_keys & dict_values and create a new dictionary with it, replacing old one. 
     * 
     * Useful for initializing and for changing the values in the dictionary from Unity UI
    */
    public void create_dict()
    {
        dict = new();

        if (dict_keys.Count != dict_values.Count)
        {
            Debug.LogError("dict_keys.Count must equal dict_values.Count");
            return;
        }

        for (int i = 0; i < dict_keys.Count; i++)
        {
            dict.Add(dict_keys[i], dict_values[i]);
        }
    }

    /*
     * Take all the values in the dictionary and output them back to dict_keys and dict_values, mainly for debugging or changing through UI
     * 
     * Need to erase old dict_keys & dict_values
     */
    public void output_dict()
    {
        dict_keys = new();
        dict_values = new();

        foreach(var item in dict)
        {
            dict_keys.Add(item.Key);
            dict_values.Add(item.Value);
        }
    }

    public Dictionary<string, T> get_dict()
    {
        return dict;
    }

    private void Update()
    {
        if (Input.GetKeyDown("i"))
        {
            create_dict();
        }
        if(Input.GetKeyDown("o"))
        {
            output_dict();
        }
    }
 }
