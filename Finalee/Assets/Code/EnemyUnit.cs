using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    public string enemyName = "Goblin";

    public int maxHP = 80;
    public int currentHP;

    [Header("Phase 1")]
    public int attackPower = 15;
    public int defense = 2;

    [Header("Phase 2")]
    public bool isPhase2 = false;
    public float phase2Threshold = 0.5f;

    public int phase2AttackPower = 25;
    public int phase2Defense = 5;

    [Header("Dialogue")]
    [TextArea] public string[] introDialogue;
    [TextArea] public string[] phase2IntroDialogue;
    [TextArea] public string[] dialogueLines;
    [TextArea] public string[] phase2DialogueLines;

    private void Awake()
    {
        currentHP = maxHP;
    }

    public int Attack()
    {
        return isPhase2 ? phase2AttackPower : attackPower;
    }

    public int GetDefense()
    {
        return isPhase2 ? phase2Defense : defense;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP < 0)
            currentHP = 0;
    }

    public string GetIntroDialogue()
    {
        if (introDialogue.Length == 0)
            return enemyName + " appears...";

        return introDialogue[Random.Range(0, introDialogue.Length)];
    }

    public string GetPhase2IntroDialogue()
    {
        if (phase2IntroDialogue.Length == 0)
            return enemyName + " becomes enraged!";

        return phase2IntroDialogue[Random.Range(0, phase2IntroDialogue.Length)];
    }

    public string GetRandomDialogue()
    {
        string[] lines = isPhase2 ? phase2DialogueLines : dialogueLines;

        if (lines.Length == 0)
            return "...";

        return lines[Random.Range(0, lines.Length)];
    }

    public bool CheckPhaseChange()
    {
        if (isPhase2)
            return false;

        float hpPercent = (float)currentHP / maxHP;

        if (hpPercent <= phase2Threshold)
        {
            isPhase2 = true;
            return true;
        }

        return false;
    }
}