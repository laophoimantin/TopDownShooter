using UnityEngine;

public class WeaponController : MonoBehaviour
{
	private Shooter _shooter;
	private WeaponVisuals _visuals;

	void Awake()
	{
		_shooter = GetComponent<Shooter>();
		_visuals = GetComponent<WeaponVisuals>();
	}

	public void AttemptFire()
	{
		
	}
}