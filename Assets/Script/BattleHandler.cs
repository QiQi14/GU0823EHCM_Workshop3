using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleHandle : MonoBehaviour
{
    [SerializeField] private Transform pfPlayerBattle;
    [SerializeField] private Transform pfEnemyBattle;

    private CharacterBattle playerCharacterBattle;
    private CharacterBattle enemyCharacterBattle;
    private CharacterBattle activeCharacterBattle;
    public State state;

    public enum State 
    {
        WaitingForPlayer,
        Busy,
    }

    private void Start()
    {
        playerCharacterBattle = SpawnCharacter(true);
        enemyCharacterBattle = SpawnCharacter(false);

        SetActiveCharacterBattle(playerCharacterBattle);
        playerCharacterBattle.state = CharacterBattle.State.Idle;
        enemyCharacterBattle.state = CharacterBattle.State.Idle;
    }



    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (playerCharacterBattle.state == CharacterBattle.State.Idle)
            {
                playerCharacterBattle.Attack(enemyCharacterBattle);
                //ChooseNextActiveCharacter();
                StartCoroutine(PerformPlayerAttack());
            }
        }


    }
    private IEnumerator PerformPlayerAttack()
    {
        // Gọi hàm Attack
        playerCharacterBattle.Attack(enemyCharacterBattle);

        // Chờ đợi cho đến khi trạng thái của playerCharacterBattle trở lại Idle
        yield return new WaitUntil(() => playerCharacterBattle.state == CharacterBattle.State.Idle);

        // Gọi hàm ChooseNextActiveCharacter
        ChooseNextActiveCharacter();
    }

    private IEnumerator PerformEnemyAttack()
    {
        // Gọi hàm Attack
        enemyCharacterBattle.Attack(playerCharacterBattle);

        // Chờ đợi cho đến khi trạng thái của playerCharacterBattle trở lại Idle
        yield return new WaitUntil(() => enemyCharacterBattle.state == CharacterBattle.State.Idle);

        // Gọi hàm ChooseNextActiveCharacter
        ChooseNextActiveCharacter();
    }

    private CharacterBattle SpawnCharacter(bool isPlayerTeam)
    {
        Vector3 position;
        if (isPlayerTeam)
        {
            position = new Vector3(-5, 0);
            pfPlayerBattle.transform.localScale = new Vector2(-1, 1);
            Transform playerTransform = Instantiate(pfPlayerBattle, position, Quaternion.identity);
            CharacterBattle playerCharacterBattle = playerTransform.GetComponent<CharacterBattle>();
            return playerCharacterBattle;
        }
        else
        {
            position = new Vector3(5, 0);
            pfEnemyBattle.transform.localScale = new Vector2(1, 1);
            Transform playerTransform = Instantiate(pfEnemyBattle, position, Quaternion.identity);
            CharacterBattle playerCharacterBattle = playerTransform.GetComponent<CharacterBattle>();
            return playerCharacterBattle;
        }
    }

    private void SetActiveCharacterBattle(CharacterBattle characterBattle)
    {
        activeCharacterBattle = characterBattle;
    }

    private void ChooseNextActiveCharacter()
    {
        if (activeCharacterBattle == playerCharacterBattle) 
        {
            SetActiveCharacterBattle(enemyCharacterBattle);
            

            if (enemyCharacterBattle.state == CharacterBattle.State.Idle)
            {
                enemyCharacterBattle.Attack(playerCharacterBattle);
                StartCoroutine(PerformEnemyAttack());
            }

        }
        else
        {
            SetActiveCharacterBattle(playerCharacterBattle);
            playerCharacterBattle.state = CharacterBattle.State.Idle;
        }
    }
}
