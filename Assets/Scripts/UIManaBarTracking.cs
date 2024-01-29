using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManaBarTracking : MonoBehaviour
{
    [SerializeField] private Slider manaSlider;
    public void UpdateManaBar(float currentValue, float maxValue)
    {
        manaSlider.value = currentValue / maxValue;
    }
}
