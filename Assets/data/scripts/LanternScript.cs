using UnityEngine;

public class LanternScript : MonoBehaviour
{
	[SerializeField] private PointRenderer pointRenderer;

	public GameObject rayPrefab;
	public float rayDistance;
	public int pointsPerBurst;
	public bool isHeld = false;
	public float hoverDistance;
	public float hoverSpeed;
	public AudioClip PickupSFX;
	public AudioSource sfx;
	public LayerMask layerMask;
	public Painter painter;
	Vector3 startPos;
	float hoverTime;
	bool hoverDir;

	private void Awake()
	{
		painter.Setup(pointRenderer, rayDistance, layerMask, rayPrefab);
	}

	void Start()
	{
		startPos = transform.position;
		hoverDir = true;
	}

	private void FixedUpdate()
	{
		for (int i = 0; i < pointsPerBurst; i++)
		{
			CreateLightPoint();
		}

		painter.Paint();

		hoverTime += hoverDir ? (Time.deltaTime * hoverSpeed) : -(Time.deltaTime * hoverSpeed);
		if (hoverTime > 1)
		{
			hoverDir = false;
			hoverTime = 1;
		}

		if (hoverTime < 0)
		{
			hoverDir = true;
			hoverTime = 0;
		}

		for (int i = 0; i < pointsPerBurst; i++)
		{
			CreateLightPoint();
		}

		if (!isHeld)
		{
			transform.position = new Vector3(startPos.x, EasingFunction.EaseInOutQuad(startPos.y - hoverDistance, startPos.y + hoverDistance, hoverTime), startPos.z);
		}
	}

	bool FireRandomPoint(out RaycastHit ray)
	{
		Vector3 dir = Random.insideUnitSphere.normalized;
		//Debug.DrawRay(transform.position, dir * range, pointColour, 2);
		return Physics.Raycast(transform.position, dir, out ray, rayDistance, layerMask);
	}

	public void CreateLightPoint()
	{
		if (FireRandomPoint(out RaycastHit hit))
		{
			CreateDotFromRaycast(hit);
		}
	}

	public void CreateDotFromRaycast(RaycastHit hit)
	{
		pointRenderer.CachePoint(hit.point);
	}

	public void Pickup(Transform holdTransform)
	{
		if (transform.parent != holdTransform)
		{
			transform.SetParent(holdTransform);
			transform.localPosition = Vector3.zero;
			isHeld = true;
			sfx.PlayOneShot(PickupSFX, 6f);
		}
	}
}