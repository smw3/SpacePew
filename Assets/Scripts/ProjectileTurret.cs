using UnityEngine;
using System.Collections;

public class ProjectileTurret : Turret {
	// Projectile specific settings
	public EntityBullet Projectile;

	public float BulletSpeed = 200f;
	public float BulletLifetime = 1f;
	public float BulletSpread = 0f;

	public float SpeedInheritanceFactor = 0.5f;


	public override void Shoot ()
	{
		if (!canFire()) return;

		// Figure out the angle the bullet will be shot in
		// Angle = Spread + Turret Rotation
		float angleSpread = Random.Range(-BulletSpread/2, BulletSpread/2);
		float currentTurretRotation = this.transform.rotation.eulerAngles.y;
		
		Quaternion BulletRotation = Quaternion.Euler(0f, angleSpread + currentTurretRotation, 0f);

		// Figure out the speed the bullet will travel at
		// Speed = Parent's Velocity * Velocity Inheritance factor + Projectile Base Speed
		Vector3 parentVelocityVector = SpeedInheritanceFactor * this.transform.parent.parent.GetComponent<Rigidbody>().velocity;
		// The base velocity vector is simply a vector of BulletSpeed magnitude in the forward direction of this (the turret) object
		Vector3 projectileBaseVelocityVector = BulletRotation * Vector3.forward * BulletSpeed;

		// Instantiate the projectile
		// Again, ONLY for projectile based weaponry
		// Lasers will need special treatment.
		EntityBullet Bullet = (EntityBullet)Instantiate(Projectile, this.transform.position, BulletRotation);

		// Tell the Bullet when it's time to disappear
		Bullet.TimeToDeath = Time.timeSinceLevelLoad + BulletLifetime;
		Bullet.ignoreObject = this.transform.parent.parent;

		// Change the Bullet Rigidbody's speed
		Rigidbody BulletRigidbody = Bullet.GetComponent<Rigidbody>();
		BulletRigidbody.AddForce(parentVelocityVector + projectileBaseVelocityVector, ForceMode.VelocityChange);

	}	

	public override float getAproxRange ()
	{
		return BulletSpeed * BulletLifetime;
	}
}
