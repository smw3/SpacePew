using UnityEngine;
using System.Collections;

public abstract class Turret : MonoBehaviour
{
	public TurretMount ParentMount;
	
	// Recoil
	protected float lastShot = -1;
	public float TimeBetweenShots = 0.5f;

    public float Spread = 0f;
	
	public abstract void Shoot();
	
	protected bool canFire() {
		// Check if the last shot was long enough ago
		if (lastShot + TimeBetweenShots < Time.timeSinceLevelLoad) {
			// We are going to fire. Save the time
			lastShot = Time.timeSinceLevelLoad;
			return true;
		}
		return false; // Weapon still cooling down
	}

	public abstract float getAproxRange();
}
