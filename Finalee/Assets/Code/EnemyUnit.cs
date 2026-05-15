using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    [Header("Enemy Info")]
    public string enemyName = "Goblin";

    [Header("Stats")]
    public int maxHP = 80;
    public int currentHP;

    public int attackPower = 15;
    public int defense = 2;

    [Header("Dialogue")]
    [TextArea(2,5)]
    public string[] dialogueLines;

    private void Awake()
    {
        currentHP = maxHP;
    }

    // ATTACK VALUE
    public int Attack()
    {
        return attackPower;
    }

    // TAKE DAMAGE
    public void TakeDamage(int damage)
    {
    currentHP -= damage;

    if (currentHP < 0)
        currentHP = 0;
    }
    // RANDOM DIALOGUE
    public string GetRandomDialogue()
    {
        if (dialogueLines.Length == 0)
            return "...";

        int randomIndex = Random.Range(0, dialogueLines.Length);

        return dialogueLines[randomIndex];
    }
}