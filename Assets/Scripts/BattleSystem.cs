using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BattleSystem : MonoBehaviour
{
    public List<CharacterController> allies;
    public List<CharacterController> enemies;

    private int currentTurnIndex = 0;
    private List<CharacterController> turnOrder = new List<CharacterController>();

    void Start()
    {
        Debug.Log("BattleSystem starting...");
        InitializeTurnOrder();
        AssignBattleSystemToCharacters();
        StartCoroutine(ExecuteTurns());
    }

    void InitializeTurnOrder()
    {
        Debug.Log("Initializing turn order...");
        turnOrder.AddRange(allies);
        turnOrder.AddRange(enemies);
        Debug.Log("Total characters in turn order: " + turnOrder.Count);
    }

    public void ApplyDamage(CharacterController attacker, CharacterController target)
    {
        float damage = attacker.characterData.Damage;

        Debug.Log(attacker.gameObject.name + " Deal " + damage + " dmg " + target.gameObject.name);

        target.characterData.HP -= damage;

        if (target.healthBar != null)
        {
            target.healthBar.UpdateHealthBar(target.characterData.HP, target.characterData.MaxHP);
        }

        if (target.manaBar != null)
        {
            Debug.Log($"Updating Mana: {target.characterData.Mana} / {target.characterData.MaxMana}");
            attacker.manaBar.UpdateManaBar(attacker.characterData.Mana, attacker.characterData.MaxMana);
        }

        if (target.characterData.HP <= 0 )
        {
            Debug.Log(target.gameObject.name + " Is killed");
            HandleDefeat(target);
        }
    }

    public CharacterController GetRandomEnemy(CharacterController attacker)
    {
        List<CharacterController> potentialTargets = new List<CharacterController>();

        potentialTargets = attacker.isAlly ? enemies : allies;

        if (potentialTargets.Count == 0)
        {
            Debug.Log(attacker.gameObject.name + " has no enemies to attack.");
            return null;
        }
        
        int randomIndex = Random.Range(0, potentialTargets.Count);

        return potentialTargets[randomIndex];
    }

    public void HandleDefeat(CharacterController defeatedCharacter)
    {
        Debug.Log(defeatedCharacter.gameObject.name + " Is Die");

        bool isAlly = defeatedCharacter.isAlly;

        if (isAlly)
        {
            allies.Remove(defeatedCharacter);
        }
        else
        {
            enemies.Remove(defeatedCharacter);
        }

        turnOrder.Remove(defeatedCharacter);
        Destroy(defeatedCharacter.gameObject);
    }

    void AssignBattleSystemToCharacters()
    {
        foreach (var character in allies)
        {
            character.Initialize(this);
        }
        foreach (var character in enemies)
        {
            character.Initialize(this);
        }
    }

    IEnumerator ExecuteTurns()
    {
        while (allies.Count > 0 && enemies.Count > 0)
        {
            if (turnOrder.Count == 0 || allies.Count == 0 || enemies.Count == 0)
            {
                yield break;
            }

            if (currentTurnIndex >= turnOrder.Count)
            {
                currentTurnIndex = 0; // Reset the index if it exceeds the count.
            }

            CharacterController currentCharacter = turnOrder[currentTurnIndex];

            if (currentCharacter != null)
            {
                Debug.Log(currentCharacter.gameObject.name + " is taking a turn");
                currentCharacter.StartTurn();

                while (currentCharacter.isActing)
                {
                    yield return null;
                }
            }

            yield return new WaitForSeconds(2); //Wait for 5 second every turn

            currentTurnIndex = (currentTurnIndex + 1) % turnOrder.Count;

            turnOrder.RemoveAll(character => character == null || character.characterData.HP <= 0);

        }
    }
}
