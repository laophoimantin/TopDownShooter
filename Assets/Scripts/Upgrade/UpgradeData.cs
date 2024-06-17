using UnityEngine;

namespace User.Scriptable.Upgrade
{
    using Manager.Upgrade;

    [CreateAssetMenu(fileName = "New Upgrade Data", menuName = "Upgrade/Upgrade Data")]
    public class UpgradeData : ScriptableObject
    {
        public UpgradeType upgradeType;
        public string upgradeName;
        public Sprite upgradeImage;
        public Sprite highlightedSprite;
    }
}
