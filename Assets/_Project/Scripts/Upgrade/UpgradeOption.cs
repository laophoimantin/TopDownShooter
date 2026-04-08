using UnityEngine;
using UnityEngine.UI;


namespace User.UI
{
    using Scriptable.Upgrade;
    using Manager.Upgrade;

    public class UpgradeOption : MonoBehaviour
    {
        [Header("REFERENCES")]
        [SerializeField] private Image image;
        [SerializeField] private Text text;
        [SerializeField] private Button targetButton;
        private UpgradeType upgradeType;
        private UpgradeManager upgradeManager;

        private void Start()
        {
            upgradeManager = FindObjectOfType<UpgradeManager>();
        }

        public void ApplyUpgrade()
        {
            upgradeManager.GetUpgradeFromType(upgradeType);
        }

        public void UpdateDisplay(UpgradeData UpgradeDta)
        {
            image.sprite = UpgradeDta.upgradeImage;
            text.text = UpgradeDta.upgradeName;
            upgradeType = UpgradeDta.upgradeType;

            SpriteState spriteState = targetButton.spriteState;
            spriteState.highlightedSprite = UpgradeDta.highlightedSprite;
            targetButton.spriteState = spriteState;
        }
    }
}
