using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarPlayer : MonoBehaviour
{
    public Slider healthbar;

    public void SetHealth(int health)
    {
        healthbar.value = health;
    }
    
}
