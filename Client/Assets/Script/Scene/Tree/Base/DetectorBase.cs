using UnityEngine;
using System.Collections;

public abstract class DetectorBase : MonoBehaviour, IDetector
{
	public Vector3 Position
	{
		get { return transform.position; }
	}

	public abstract bool IsDetected(Bounds bounds, out bool isInView);

	public abstract bool IsDetected(Bounds bounds);
}
