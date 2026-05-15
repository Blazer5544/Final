using UnityEngine;

public static class DamageCalculator
{
    public static int CalculateDamage(int attackPower, int defense, float variance = 0.2f)
    {
        // Step 1: Apply defense (simple subtraction model)
        float damage = attackPower - defense;

        if (damage < 1)
            damage = 1;

        // Step 2: Add fluctuation
        float min = damage * (1f - variance);
        float max = damage * (1f + variance);

        float finalDamage = Random.Range(min, max);

        return Mathf.RoundToInt(finalDamage);
    }
}