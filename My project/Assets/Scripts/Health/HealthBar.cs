using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health; /// sets max value of the slider
        slider.value = health; /// sets inital health to max health
    }

    public void SetHealth(int health)
    {
        slider.value = health; /// sets value of the slider
    }

}
