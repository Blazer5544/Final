using System.Collections;
using UnityEngine;
using TMPro;

public class BattleManager : MonoBehaviour
{
    // ============================================
    // BATTLE STATES
    // ============================================

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

    // ============================================
    // REFERENCES
    // ============================================

    [Header("Battle Units")]
    public PlayerUnit playerUnit;
    public EnemyUnit enemyUnit;

    [Header("UI")]
    public TMP_Text dialogueText;
    public TMP_Text playerHPText;
    public TMP_Text enemyHPText;

    // ============================================
    // START BATTLE
    // ============================================

    private void Start()
    {
        StartCoroutine(StartBattle());
    }

    IEnumerator StartBattle()
    {
        state = BattleState.START;

        dialogueText.text =
            "A wild " +
            enemyUnit.enemyName +
            " appeared!";

        UpdateUI();

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYER_TURN;

        PlayerTurn();
    }

    // ============================================
    // PLAYER TURN
    // ============================================

    void PlayerTurn()
    {
        dialogueText.text =
            playerUnit.playerName +
            "'s Turn\nChoose an action.";
    }

    // ============================================
    // PLAYER ATTACK
    // ============================================

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYER_TURN)
            return;

        StartCoroutine(PlayerAttackRoutine());
    }

    IEnumerator PlayerAttackRoutine()
    {
        int damage = playerUnit.Attack();

        bool enemyDead =
            enemyUnit.TakeDamage(damage);

        dialogueText.text =
            playerUnit.playerName +
            " attacks " +
            enemyUnit.enemyName +
            " for " +
            damage +
            " damage!";

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

            StartCoroutine(EnemyTurnRoutine());
        }
    }

    // ============================================
    // PLAYER TALK
    // ============================================

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
            enemyUnit.enemyName +
            " says:\n\"" +
            enemyUnit.GetRandomDialogue() +
            "\"";

        yield return new WaitForSeconds(3f);

        state = BattleState.ENEMY_TURN;

        StartCoroutine(EnemyTurnRoutine());
    }

    // ============================================
    // ENEMY TURN
    // ============================================

    IEnumerator EnemyTurnRoutine()
    {
        dialogueText.text =
            enemyUnit.enemyName +
            " attacks!";

        yield return new WaitForSeconds(1f);

        int damage = enemyUnit.Attack();

        bool playerDead =
            playerUnit.TakeDamage(damage);

        dialogueText.text =
            enemyUnit.enemyName +
            " dealt " +
            damage +
            " damage!";

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

    // ============================================
    // END BATTLE
    // ============================================

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueText.text =
                "You defeated " +
                enemyUnit.enemyName +
                "!";
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text =
                playerUnit.playerName +
                " has fallen...";
        }
    }

    // ============================================
    // UPDATE UI
    // ============================================

    void UpdateUI()
    {
        playerHPText.text =
            playerUnit.playerName +
            "\nHP: " +
            playerUnit.currentHP +
            "/" +
            playerUnit.maxHP;

        enemyHPText.text =
            enemyUnit.enemyName +
            "\nHP: " +
            enemyUnit.currentHP +
            "/" +
            enemyUnit.maxHP;
    }
}