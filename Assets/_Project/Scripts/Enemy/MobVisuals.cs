using UnityEngine;

public class MobVisuals : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _visual;

    public void SetFacingLeft(bool isLeft)
    {
        if (_visual.flipX != isLeft)
        {
            _visual.flipX = isLeft;
        }
    }
}