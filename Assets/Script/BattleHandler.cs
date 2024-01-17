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
        state = State.WaitingForPlayer; 
    }



    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerCharacterBattle.Attack(enemyCharacterBattle);
        }


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
}
