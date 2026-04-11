using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class UpgradeSelectButton : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Button _targetButton;
    
    private UpgradeData _currentData; 
    private UpgradePanel _parentPanel;
    private PlayerController  _player;

    public void Setup(UpgradeData data, UpgradePanel panel, PlayerController target)
    {
        _currentData = data;
        _parentPanel = panel;
        _player = target;
        
        _image.sprite = data.UpgradeImage;
        _text.text = data.upgradeName;

        SpriteState spriteState = _targetButton.spriteState;
        spriteState.highlightedSprite = data.HighlightedSprite;
        _targetButton.spriteState = spriteState;
        
        _targetButton.onClick.RemoveAllListeners();
        _targetButton.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        if (_player != null)
        {
            _currentData.ApplyUpgrade(_player);
            _parentPanel.HidePanel();
        }
    }
}