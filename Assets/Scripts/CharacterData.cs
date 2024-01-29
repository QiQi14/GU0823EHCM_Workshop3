using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObject", menuName = "ScriptableObjects/Character")]
public class CharacterData : ScriptableObject
{
    [SerializeField]
    private float defaultHP;
    public float MaxHP;
    float hp;
    public float HP 
    {  
        get { return hp; } 
        set { hp = Mathf.Clamp(value, 0, MaxHP); } 
    }

    [SerializeField]
    private float defaultMana;
    public float MaxMana = 100f;
    float mana;
    public float Mana 
    { 
        get {  return mana; } 
        set { mana = Mathf.Clamp(value, 0, MaxMana); } 
    }

    [SerializeField]
    private float defaultDamage;
    float damage;
    public float Damage { get { return damage; } set { damage = value; } }

    public void GainMana(float amount)
    {
        Mana += amount;
    }

    private void OnEnable()
    {
        hp = defaultHP;
        MaxHP = defaultHP;
        mana = defaultMana;
        damage = defaultDamage;
    }
}
