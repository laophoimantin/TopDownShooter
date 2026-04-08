using UnityEngine;

public class AimController : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private Camera _mainCam;
	[SerializeField] private Transform _handPos;

	private bool isFacingRight = true;

	void Update()
	{
		AimAndFlip();
	}

	private void AimAndFlip()
	{
		Vector3 mousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = 0;

		Vector3 aimDirection = (mousePos - transform.position).normalized;
		float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

		_handPos.rotation = Quaternion.Euler(0, 0, angle);

		if ((mousePos.x < transform.position.x && isFacingRight) ||
			(mousePos.x > transform.position.x && !isFacingRight))
		{
			isFacingRight = !isFacingRight;
			Vector3 localScale = transform.localScale;
			localScale.x *= -1f;
			transform.localScale = localScale;
			_handPos.localScale = new Vector3(_handPos.localScale.x, _handPos.localScale.y * -1, _handPos.localScale.z);
		}
	}
}