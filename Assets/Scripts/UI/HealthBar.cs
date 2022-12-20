using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

	public Slider slider;

    public void SetHealth(float currentHp, float maxHp)
	{
        float newVal = currentHp / maxHp;
        if (newVal < 0)
        {
            newVal = 0;
        }
		slider.value = newVal;
	}

}
