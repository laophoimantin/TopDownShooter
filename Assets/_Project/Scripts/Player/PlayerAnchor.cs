using UnityEngine;

[CreateAssetMenu(menuName = "References/Player Anchor")]
public class PlayerAnchor : ScriptableObject
{
    public PlayerController Value { get; private set; }
    public bool IsReady => Value != null;

    public void SetPlayer(PlayerController anchor)
    {
        Value = anchor;
    }

    public void Clear()
    {
        Value = null;
    }
}