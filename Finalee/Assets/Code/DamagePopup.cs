using System.Collections;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    public TMP_Text text;

    public float floatSpeed = 2f;
    public float duration = 1f;

    private Vector3 moveDirection;

    void Awake()
    {
        if (text == null)
            text = GetComponent<TMP_Text>();
    }

    public void Setup(int damageAmount)
    {
        text.text = damageAmount.ToString();

        moveDirection = new Vector3(
            Random.Range(-0.3f, 0.3f),
            1f,
            0f
        );

        StartCoroutine(FadeAndMove());
    }

    IEnumerator FadeAndMove()
    {
        float time = 0f;
        Color originalColor = text.color;

        while (time < duration)
        {
            transform.position += moveDirection * floatSpeed * Time.deltaTime;

            time += Time.deltaTime;

            float alpha = Mathf.Lerp(1f, 0f, time / duration);

            text.color = new Color(
                originalColor.r,
                originalColor.g,
                originalColor.b,
                alpha
            );

            yield return null;
        }

        Destroy(gameObject);
    }
}