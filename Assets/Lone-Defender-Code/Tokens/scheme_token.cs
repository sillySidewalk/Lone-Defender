using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class scheme_token : LD_token
{
    public override string display_type => "scheme_token";

    protected scheme parent_scheme;

    public abstract void investigate();
}
