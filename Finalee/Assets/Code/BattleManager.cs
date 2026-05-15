using System.Collections;
using UnityEngine;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public enum BattleState
    {
        START,
        PLAYER_TURN,
        ENEMY_TURN,
        DIALOGUE,
        WON,
        LOST
    }

    public BattleState state;

    [Header("Units")]
    public PlayerUnit playerUnit;
    public EnemyUnit enemyUnit;

    [Header("UI")]
    public TMP_Text dialogueText;
    public TMP_Text playerHPText;
    public TMP_Text enemyHPText;

    private void Start()
    {
        StartCoroutine(StartBattle());
    }

    IEnumerator StartBattle()
    {
        state = BattleState.START;

        dialogueText.text =
            "A wild " + enemyUnit.enemyName + " appears!";

        UpdateUI();

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYER_TURN;

        PlayerTurn();
    }

    void PlayerTurn()
    {
        dialogueText.text =
            playerUnit.playerName + "'s turn.";
    }

    // =========================
    // PLAYER ATTACK
    // =========================

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYER_TURN)
            return;

        StartCoroutine(PlayerAttack());
    }

    IEnumerator PlayerAttack()
    {
        int rawAttack = playerUnit.Attack();

        int damage = DamageCalculator.CalculateDamage(
        rawAttack,
        enemyUnit.defense,
        0.2f
        );

        enemyUnit.TakeDamage(damage);

        bool enemyDead = enemyUnit.currentHP <= 0;

        dialogueText.text =
            playerUnit.playerName +
            " deals " + damage + " damage!";

        UpdateUI();

        yield return new WaitForSeconds(2f);

        if (enemyDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMY_TURN;
            StartCoroutine(EnemyTurn());
        }
    }

    // =========================
    // TALK SYSTEM
    // =========================

    public void OnTalkButton()
    {
        if (state != BattleState.PLAYER_TURN)
            return;

        StartCoroutine(TalkRoutine());
    }

    IEnumerator TalkRoutine()
    {
        state = BattleState.DIALOGUE;

        dialogueText.text =
            enemyUnit.enemyName + ": \"" +
            enemyUnit.GetRandomDialogue() + "\"";

        yield return new WaitForSeconds(3f);

        state = BattleState.ENEMY_TURN;

        StartCoroutine(EnemyTurn());
    }

    // =========================
    // ENEMY TURN
    // =========================

    IEnumerator EnemyTurn()
    {
        dialogueText.text =
            enemyUnit.enemyName + " attacks!";

        yield return new WaitForSeconds(1f);

        int rawAttack = enemyUnit.Attack();

        int damage = DamageCalculator.CalculateDamage(
        rawAttack,
        playerUnit.defense,
        0.2f
        );

        playerUnit.TakeDamage(damage);

        bool playerDead = playerUnit.currentHP <= 0;

        dialogueText.text =
            enemyUnit.enemyName +
            " deals " + damage + " damage!";

        UpdateUI();

        yield return new WaitForSeconds(2f);

        if (playerDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYER_TURN;
            PlayerTurn();
        }
    }

    // =========================
    // END BATTLE
    // =========================

    void EndBattle()
    {
        if (state == BattleState.WON)
            dialogueText.text = "You win!";
        else if (state == BattleState.LOST)
            dialogueText.text = "You were defeated...";
    }

    // =========================
    // UI
    // =========================

    void UpdateUI()
    {
        playerHPText.text =
            playerUnit.playerName +
            " HP: " + playerUnit.currentHP +
            "/" + playerUnit.maxHP;

        enemyHPText.text =
            enemyUnit.enemyName +
            " HP: " + enemyUnit.currentHP +
            "/" + enemyUnit.maxHP;
    }
}