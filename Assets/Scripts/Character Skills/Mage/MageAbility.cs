using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageAbility : MonoBehaviour, ISpecialAbility
{
    public void ExecuteSpecialAbility()
    {
        Debug.Log("Mage uses its Fire Ball!");
    }

    public void PerformSpecialDamage()
    {

    }
}
