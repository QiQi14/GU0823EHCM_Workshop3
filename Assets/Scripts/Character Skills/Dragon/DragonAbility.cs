using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAbility : MonoBehaviour, ISpecialAbility
{
    private CharacterController characterController;

    [SerializeField] private float specialDamage = 50f;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    public void ExecuteSpecialAbility()
    {
        Debug.Log("Dragon uses its mighty roar!");
        //characterController.TriggerAnimation("DragonSpecial");
        PerformSpecialDamage();
    }

    public void PerformSpecialDamage()
    {
        
    }
}
