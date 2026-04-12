using UnityEngine;

public class AimController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera _mainCam;
    [SerializeField] private Transform _weaponPivot;
    [SerializeField] private Transform _handPos;
    [SerializeField] private Transform _playerVisual;
    
    private Vector3 _aimDir;
    public Vector3 AimDir => _aimDir;

    private bool _isFacingRight = true;

    void Update()
    {
        AimAndFlip();
    }

    private void AimAndFlip()
    {
        Vector3 mouseWorldPos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        _aimDir = (mouseWorldPos - transform.position).normalized;
        float angle = Mathf.Atan2(_aimDir.y, _aimDir.x) * Mathf.Rad2Deg;

        _weaponPivot.rotation = Quaternion.Euler(0, 0, angle);

        GunFlipHandle(mouseWorldPos.x);
    }

    private void GunFlipHandle(float targetX)
    {
        bool shouldFaceRight = targetX > transform.position.x;

        if (shouldFaceRight != _isFacingRight)
        {
            FlipAll();
        }
    }

    private void FlipAll()
    {
        _isFacingRight = !_isFacingRight;
        
        Vector3 handScale = _handPos.localScale;
        handScale.y *= -1;
        _handPos.localScale = handScale;
        
        if (_playerVisual != null)
        {
            Vector3 visualScale = _playerVisual.localScale;
            visualScale.x *= -1;
            _playerVisual.localScale = visualScale;
        }
    }
}