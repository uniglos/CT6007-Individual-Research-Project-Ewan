using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DDABars : MonoBehaviour
{
    public Slider combatBar;
    public Slider navBar;
    public Slider ddaSkewBar;

    public void SetDDABars(float cmbtAssist, float navAssist, float ddaSkew)
    {
        float total = cmbtAssist + navAssist;
        combatBar.maxValue = total;
        navBar.maxValue = total;

        combatBar.value = cmbtAssist;
        navBar.value = navAssist;
        ddaSkewBar.value = ddaSkew;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
