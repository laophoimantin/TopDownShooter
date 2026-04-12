using UnityEngine;
using UnityEngine.UI;

public class HeartDisplay : MonoBehaviour
{
    [SerializeField] private Image[] _hearts;
    [SerializeField] private Sprite _fullHeart;
    [SerializeField] private Sprite _emptyHeart;

    void OnEnable()
    {
        this.Subscribe<OnPlayerHealthChange>(UpdateHearts);
    }

    void OnDisable()
    {
        if (EventDispatcher.Instance != null)
        {
            this.Unsubscribe<OnPlayerHealthChange>(UpdateHearts);
        }
    }

    private void UpdateHearts(OnPlayerHealthChange eventData)
    {
        int currentHealth = eventData.CurrentHealth;
        int maxHealth =  eventData.MaxHealth;
        
        for (int i = 0; i < _hearts.Length; i++)
        {
            if (i < maxHealth)
            {
                _hearts[i].enabled = true;

                _hearts[i].sprite = (i < currentHealth) ? _fullHeart : _emptyHeart;
            }
            else
            {
                _hearts[i].enabled = false;
            }
        }
        
    }
}