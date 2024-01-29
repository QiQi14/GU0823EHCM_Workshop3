using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public CharacterData characterData;
    private Animator animator;


    [HideInInspector]
    public Transform target;
    [HideInInspector]
    public BattleSystem battleSystem;

    
    public UIHealthBarTracking healthBar;
    public UIManaBarTracking manaBar;

    private Vector3 originalPos;
    public bool isAlly;
    public bool isActing = false;

    public float moveSpeed = 10f;

    private IEnumerator currentAction;

    void Awake()
    {
        animator = GetComponent<Animator>();
        manaBar.UpdateManaBar(characterData.Mana, characterData.MaxMana);
    }

    public void Initialize(BattleSystem system)
    {
        battleSystem = system;
    }

    public void StartTurn()
    {
        originalPos = transform.position;
        currentAction = ActingSequence();
        StartCoroutine(currentAction);
    }

    public void EndTurn()
    {
        isActing = false;
    }

    public void AttackTarget()
    {
        if (target == null)
        {
            Debug.Log(gameObject.name + " is attempting to attack " + target.gameObject.name);
            return;
        }

        Debug.Log(gameObject.name + " Is attacking " + target.gameObject.name);

        float damage = characterData.Damage;

        CharacterController targetCharacter = target.GetComponent<CharacterController>();

        if (targetCharacter != null)
        {
            battleSystem.ApplyDamage(this, targetCharacter);
            GainManaOnAttack();
        }

        target = null; //Reset target after attacked to avoid loop attack

        isActing = false;

        StartCoroutine(CheckAndExecuteSpecialAbility());
    }

    public void PerformAttack()
    {
        CharacterController targetCharacter = battleSystem.GetRandomEnemy(this);

        if (targetCharacter == null)
        {
            Debug.Log(gameObject.name + " finds no target to attack.");
            return;
        }

        target = targetCharacter.transform;
    }

    void GainManaOnAttack()
    {
        characterData.GainMana(10);
        UpdateManaUI();
    }

    public void UpdateManaUI()
    {
        if (manaBar != null)
        {
            manaBar.UpdateManaBar(characterData.Mana, characterData.MaxMana);
        }
    }

    private IEnumerator ActingSequence()
    {
        Debug.Log(gameObject.name + " starts ActingSequence.");

        isActing = true;

        PerformAttack();

        yield return MoveToTargetPos();
        Debug.Log(gameObject.name + " reached the target.");

        yield return AttemptAttack();
        Debug.Log(gameObject.name + " attempted an attack.");

        yield return new WaitForSeconds(1f); // Waiting after attack

        yield return MoveBackToOriginalPos();
        Debug.Log(gameObject.name + " moved back to original position.");

        EndTurn();
        Debug.Log(gameObject.name + " ended its turn.");
    }

    private IEnumerator AttemptAttack()
    {
        Debug.Log(gameObject.name + " is attempting to attack.");

        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            Debug.Log($"{gameObject.name} is {distance} units away from {target.gameObject.name}.");

            if (distance < 1)
            {
                string attackTrigger = "";
                if (gameObject.CompareTag("Dragon"))
                {
                    attackTrigger = "DragonAttack";
                } else if (gameObject.CompareTag("Reptile"))
                {
                    attackTrigger = "ReptileAttack";
                } else if (gameObject.CompareTag("Warrior"))
                {
                    attackTrigger = "WarriorAttack";
                } else if (gameObject.CompareTag("Mage"))
                {
                    attackTrigger = "MageAttack";
                }

                if(!string.IsNullOrEmpty(attackTrigger))
                {
                    animator.SetTrigger(attackTrigger);
                }

                AttackTarget();
            }
        }

        yield return null;
    }

    private IEnumerator MoveToTargetPos()
    {
        while (target != null && Vector3.Distance(transform.position, target.position) > 0.1f)
        {
            float step = Mathf.Clamp(moveSpeed * Time.deltaTime, 0, 0.1f);
            Debug.Log($"{gameObject.name} move speed: {step}");
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
            yield return null;
        }
    }

    private IEnumerator MoveBackToOriginalPos()
    {
        while (Vector3.Distance(transform.position, originalPos) > 0.1f)
        {
            float stepBack = Mathf.Clamp(moveSpeed * Time.deltaTime, 0, 0.1f);
            transform.position = Vector3.MoveTowards(transform.position, originalPos, stepBack);
            yield return null; // wait until next frame
        }
    }

    IEnumerator CheckAndExecuteSpecialAbility()
    {
        yield return new WaitForSeconds(2f);

        if (characterData.Mana >= characterData.MaxMana)
        {
            ISpecialAbility castSkill = GetComponent<ISpecialAbility>();

            if (castSkill != null)
            {
                castSkill.ExecuteSpecialAbility();
                characterData.Mana = 0;
                UpdateManaUI();
            }
        }
    }
}
