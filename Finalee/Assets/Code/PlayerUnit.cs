using UnityEngine;

public class PlayerUnit : MonoBehaviour
{
    [Header("Player Info")]
    public string playerName = "Hero";

    [Header("Stats")]
    public int maxHP = 100;
    public int currentHP;

    public int attackPower = 20;
    public int defense = 5;

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
    public bool TakeDamage(int damage)
    {
        damage -= defense;

        if (damage < 1)
            damage = 1;

        currentHP -= damage;

        if (currentHP < 0)
            currentHP = 0;

        return currentHP <= 0;
    }

    // HEAL
    public void Heal(int amount)
    {
        currentHP += amount;

        if (currentHP > maxHP)
            currentHP = maxHP;
    }
}