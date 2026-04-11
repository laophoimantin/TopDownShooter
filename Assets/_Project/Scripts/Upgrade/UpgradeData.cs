using UnityEngine;


public abstract class UpgradeData : ScriptableObject
{
    [Header("UI Info")]
    public string upgradeName;
    [SerializeField] protected float _power;
    public Sprite UpgradeImage;
    public Sprite HighlightedSprite;

    public abstract void ApplyUpgrade(PlayerController player);
}