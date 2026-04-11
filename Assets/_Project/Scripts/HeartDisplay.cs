using UnityEngine;
using UnityEngine.UI;

public class HeartDisplay : MonoBehaviour
{
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    void OnEnable()
    {
        PlayerHealth.OnHealthChanged += UpdateHearts;
    }

    void OnDisable()
    {
        PlayerHealth.OnHealthChanged -= UpdateHearts;
    }

    private void UpdateHearts(int currentHealth, int maxHealth)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < maxHealth)
            {
                hearts[i].enabled = true;

                hearts[i].sprite = (i < currentHealth) ? fullHeart : emptyHeart;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
}