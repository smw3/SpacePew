using UnityEngine;
using System.Collections;

public interface ILaserTarget
{
	void OnLaserHit(Vector3 collisionPoint, float intensity);
}