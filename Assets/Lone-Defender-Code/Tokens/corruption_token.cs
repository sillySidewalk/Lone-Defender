using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class corruption_token : LD_token
{
    public override string token_name { get; } = "corruption token";

    public override Dictionary<string, List<int>> display_position
    {
        get
        {
            return new Dictionary<string, List<int>>
            {
                { "Clearing", new List<int>(){ 0 } },
            };
        }
    }
}
