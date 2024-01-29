using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorAbility : MonoBehaviour, ISpecialAbility
{
    public void ExecuteSpecialAbility()
    {
        Debug.Log("Warrior uses its Strong Attack!");
    }

    public void PerformSpecialDamage()
    {

    }
}
