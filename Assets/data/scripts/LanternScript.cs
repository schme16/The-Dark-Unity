using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.VFX;

public class LanternScript : MonoBehaviour
{
	[SerializeField] private PointRenderer pointRenderer;

	public GameManager gm;
	public GameObject rayPrefab;
	public float rayDistance;
	public int pointsPerBurst;
	public bool isHeld = false;
	public float hoverDistance;
	public float hoverSpeed;
	public int particleBurstAmount;
	public float particleBurstDistance;
	public AudioClip PickupSFX;
	public AudioClip BurstSFX;
	public AudioSource sfx;
	public LayerMask layerMask;
	public Painter painter;
	public VisualEffect lanternVFX;
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

	public async void CreateLightPoint()
	{
		if (FireRandomPoint(out RaycastHit hit))
		{
			CreateDotFromRaycast(hit);
		}

		await UniTask.Yield();
	}

	public async void CreateDotFromRaycast(RaycastHit hit)
	{
		pointRenderer.CachePoint(hit.point);
		await UniTask.Yield();
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

	public async void ParticleBurst()
	{
		float oldRayDistance = rayDistance;
		rayDistance = particleBurstDistance;
		for (int i = 0; i < particleBurstAmount; i++)
		{
			CreateLightPoint();
		}

		sfx.PlayOneShot(BurstSFX, 1f);

		rayDistance = oldRayDistance;
		await UniTask.Yield();
	}
}