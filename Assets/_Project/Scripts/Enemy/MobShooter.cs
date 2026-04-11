using UnityEngine;

public class MobShooter : MonoBehaviour
{
    private MobData _data;
    private Transform _target;
    private float _fireTimer;

    public void Init(MobData data, Transform target)
    {
        _data = data;
        _target = target;
        _fireTimer = _data.fireRate; 
    }

    void Update()
    {
        if (_target == null || !_data.ranger) return;

        _fireTimer -= Time.deltaTime;
        if (_fireTimer <= 0)
        {
            Shoot();
            _fireTimer = _data.fireRate;
        }
    }

    private void Shoot()
    {
        // Nhớ chuyển audioSource Play qua SoundManager đi nhé, tôi không viết vào đây đâu
        Vector3 direction = (_target.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        GameObject bullet = Instantiate(_data.bulletPf, transform.position, Quaternion.Euler(0, 0, angle));
        
        if (bullet.TryGetComponent(out Projectile proj))
        {
            proj.Setup(1, _data.bulletSpeed, 5f, 1, 0f); 
        }
    }
}