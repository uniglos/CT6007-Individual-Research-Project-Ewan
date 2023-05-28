using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReasonForDeath 
{
    string cause;
    int amountOfDeaths = 0;
    public bool needForDDA;

    public ReasonForDeath(string _cause)
    {
        cause = _cause;
        amountOfDeaths += 1;

    }
}
