using UnityEngine;
using System.Collections;

public class LaserTurret : Turret {
	// laser specific settings
	public Laser LaserPrefab;

	public float LaserSpread = 0f;
	public float LaserMaxRange = 100f;
	
	public override void Shoot ()
	{
		if (!canFire()) return;

		// Figure out the angle the bullet will be shot in
		// Angle = Spread + Turret Rotation
		float angleSpread = Random.Range(-LaserSpread/2, LaserSpread/2);
		float currentTurretRotation = this.transform.rotation.eulerAngles.y;

		Quaternion LaserRotation = Quaternion.Euler(0f, angleSpread + currentTurretRotation, 0f);
				
		Laser laserEntity = (Laser)Instantiate(LaserPrefab, this.transform.position, LaserRotation);
		laserEntity.transform.parent = this.transform;
		laserEntity.maxLength = LaserMaxRange;

	}	

	public override float getAproxRange ()
	{
		return LaserMaxRange;
	}
}
