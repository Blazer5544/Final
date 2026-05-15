using UnityEngine;

public class PlayerUnit : MonoBehaviour
{
    public string playerName = "Hero";

    public int maxHP = 100;
    public int currentHP;

    public int attackPower = 20;
    public int defense = 5;

    private void Awake()
    {
        currentHP = maxHP;
    }

    public int Attack()
    {
        return attackPower;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP < 0)
            currentHP = 0;
    }
}