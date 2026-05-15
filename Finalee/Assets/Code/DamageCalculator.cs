using UnityEngine;

public static class DamageCalculator
{
    public static int CalculateDamage(int attackPower, int defense, float variance = 0.2f)
    {
        float damage = attackPower - defense;

        if (damage < 1)
            damage = 1;

        float min = damage * (1f - variance);
        float max = damage * (1f + variance);

        return Mathf.RoundToInt(Random.Range(min, max));
    }
}