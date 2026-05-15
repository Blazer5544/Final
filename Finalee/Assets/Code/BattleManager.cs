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

    [Header("Damage Popup")]
    public GameObject damagePopupPrefab;

    private bool isDefending;
    private float defendMultiplier = 0.5f;

    private void Start()
    {
        StartCoroutine(StartBattle());
    }

    // =========================
    // START BATTLE
    // =========================

    IEnumerator StartBattle()
    {
        state = BattleState.START;

        UpdateUI();

        yield return new WaitForSeconds(0.5f);

        dialogueText.text = enemyUnit.enemyName + " approaches...";

        yield return new WaitForSeconds(1.5f);

        dialogueText.text = enemyUnit.GetIntroDialogue();

        yield return new WaitForSeconds(3f);

        dialogueText.text =
            "A wild " + enemyUnit.enemyName + " appears!";

        yield return new WaitForSeconds(1.5f);

        state = BattleState.PLAYER_TURN;
        PlayerTurn();
    }

    void PlayerTurn()
    {
        dialogueText.text = "Choose your action.";
    }

    // =========================
    // ATTACK
    // =========================

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYER_TURN)
            return;

        StartCoroutine(PlayerAttack());
    }

    IEnumerator PlayerAttack()
    {
        int raw = playerUnit.Attack();

        int damage = DamageCalculator.CalculateDamage(
            raw,
            enemyUnit.GetDefense()
        );

        enemyUnit.TakeDamage(damage);

        SpawnDamagePopup(damage, enemyUnit.transform);

        if (enemyUnit.CheckPhaseChange())
        {
            yield return StartCoroutine(PhaseChangeDialogue());
        }

        bool enemyDead = enemyUnit.currentHP <= 0;

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
    // DEFEND
    // =========================

    public void OnDefendButton()
    {
        if (state != BattleState.PLAYER_TURN)
            return;

        isDefending = true;

        dialogueText.text = playerUnit.playerName + " is guarding!";

        state = BattleState.ENEMY_TURN;
        StartCoroutine(EnemyTurn());
    }

    // =========================
    // ENEMY TURN
    // =========================

    IEnumerator EnemyTurn()
    {
        dialogueText.text = enemyUnit.enemyName + " attacks!";

        yield return new WaitForSeconds(1f);

        int raw = enemyUnit.Attack();

        int damage = DamageCalculator.CalculateDamage(
            raw,
            playerUnit.defense
        );

        if (isDefending)
        {
            damage = Mathf.RoundToInt(damage * defendMultiplier);
        }

        playerUnit.TakeDamage(damage);

        SpawnDamagePopup(damage, playerUnit.transform);

        isDefending = false;

        bool playerDead = playerUnit.currentHP <= 0;

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
    // PHASE TRANSITION
    // =========================

    IEnumerator PhaseChangeDialogue()
    {
        state = BattleState.DIALOGUE;

        dialogueText.text = enemyUnit.GetPhase2IntroDialogue();

        yield return new WaitForSeconds(3f);

        dialogueText.text = enemyUnit.enemyName + " enters SECOND PHASE!";

        yield return new WaitForSeconds(2f);
    }

    // =========================
    // POPUP SPAWNER
    // =========================

    void SpawnDamagePopup(int damage, Transform target)
    {
        Vector3 pos = target.position;

        GameObject popup = Instantiate(
            damagePopupPrefab,
            pos,
            Quaternion.identity,
            transform
        );

        popup.GetComponent<DamagePopup>().Setup(damage);
    }

    // =========================
    // END
    // =========================

    void EndBattle()
    {
        dialogueText.text =
            state == BattleState.WON ? "Victory!" : "You were defeated...";
    }

    // =========================
    // UI
    // =========================

    void UpdateUI()
    {
        playerHPText.text =
            playerUnit.playerName + " HP: " +
            playerUnit.currentHP + "/" + playerUnit.maxHP;

        enemyHPText.text =
            enemyUnit.enemyName + " HP: " +
            enemyUnit.currentHP + "/" + enemyUnit.maxHP;
    }
}