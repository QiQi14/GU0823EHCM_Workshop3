using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBattle : MonoBehaviour
{
    private Character_Base characterBase;
    private bool Attacking = true;
    public State state;
    private Vector3 slideTargetPosition;
    private Action onSlideComplete;
    public Vector3 startingPosition;
    private GameObject selectionCircleGameObject;
    private HealthSystem healthSystem;
    public GameObject healthBar;

    public enum State
    {
        Idle,
        Sliding,
        Busy,
    }

    private void Awake()
    {
        characterBase = GetComponent<Character_Base>();
        selectionCircleGameObject = transform.Find("SelectionCircle").gameObject;
        HideSelectionCircle();
        state = State.Idle;
    }

    private void Start()
    {
        characterBase.PlayIdleAnimation(new Vector3(1, 0));
        startingPosition = GetPosition();

        healthSystem = new HealthSystem(100);
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChange;
    }

    private void HealthSystem_OnHealthChange(object sender, EventArgs e)
    {
        healthBar.transform.localScale = new Vector3 (healthSystem.GetHealthPercent(), healthBar.transform.localScale.y, healthBar.transform.localScale.z);
    }

    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                break;
            case State.Busy:
                break;
            case State.Sliding:
                characterBase.PlayWalkingAnimation(new Vector3(1, 0));
                float slideSpeed = 2f;
                transform.position += (slideTargetPosition - GetPosition()) * slideSpeed * Time.deltaTime;

                float reachDistance = 0.5f;
                if (Vector3.Distance(GetPosition(), slideTargetPosition) < reachDistance)
                {
                    transform.position = slideTargetPosition;
                    onSlideComplete();
                }
                break;
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void OnAttackCompleted()
    {
        SlideToPosition(startingPosition, () =>
        {
            characterBase.PlayIdleAnimation(new Vector3(1, 0));
            Attacking = true;
            state = State.Idle;
            if (startingPosition.x < 0)
            {
                characterBase.SetTransform(true);
            } else
            {
                characterBase.SetTransform(false);
            }

        });
    }

    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
        DamagePopup.Create(GetPosition(), damageAmount, false);

        if (healthSystem.IsDead())
        {
            characterBase.PlayDeadAnimation(new Vector3(1, 0));
        }
    }
    public bool IsDead()
    {
        return healthSystem.IsDead();
    }

    public void OnPlayerDead()
    {

        state = State.Busy;
    }

    public void Attack(CharacterBattle targetCharacterBattle)
    {
        
        Vector3 slideTargetPosition = targetCharacterBattle.GetPosition() + (GetPosition() - targetCharacterBattle.GetPosition()).normalized * 1f;
        


        SlideToPosition(slideTargetPosition, () =>
        {
            Vector3 attackDir = (targetCharacterBattle.GetPosition() - GetPosition()).normalized;
            if (Attacking == true)
            {
                Attacking = false;
                characterBase.PlayAttackAnimation(attackDir);
                int damageAmount = UnityEngine.Random.Range(20, 50);
                targetCharacterBattle.Damage(damageAmount);
            }
        });

    }

    private void SlideToPosition(Vector3 slideTargetPosition, Action onSlideComplete)
    {
        
        this.slideTargetPosition = slideTargetPosition;
        this.onSlideComplete = onSlideComplete;
        state = State.Sliding;
        if(slideTargetPosition.x > 0)
        {
            characterBase.SetTransform(true);
        } else
        {
            characterBase.SetTransform(false);
        }
    }

    public void HideSelectionCircle()
    {
        selectionCircleGameObject.SetActive(false);
    }

    public void ShowSelectionCircle()
    {
        selectionCircleGameObject.SetActive(true);
    }
}
