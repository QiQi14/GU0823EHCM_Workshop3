using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReptileAbility : MonoBehaviour, ISpecialAbility
{
    public void ExecuteSpecialAbility()
    {
        Debug.Log("Reptile uses its Poison!");
    }

    public void PerformSpecialDamage()
    {

    }
}
