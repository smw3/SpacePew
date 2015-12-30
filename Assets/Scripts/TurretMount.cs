using UnityEngine;
using System.Collections;

public class TurretMount : MonoBehaviour {

	public bool Armed;
	public bool Automated;
	
	public GameObject Target;

	public float minAngle, maxAngle;
	public GameObject TurretPrefab;

	private Turret Turret;

	void Start () {
	
	}

	public void Populate ()
	{
		GameObject TurretObject = (GameObject)Instantiate(TurretPrefab);
		Turret = (Turret)TurretObject.GetComponent<Turret>();

		Turret.ParentMount = this.GetComponent<TurretMount>();

		Turret.transform.parent = this.transform;
		Turret.transform.localPosition = Vector3.zero;
	}

	public void manualShoot() {
		if (!Automated) Shoot();
	}

	public void Shoot ()
	{
		if (!Armed) return;
		if (Turret) {
			Turret.GetComponent<Turret>().Shoot();
		}
	}

	void Update () {
		if (!Turret) return;

		if (Automated) {
			DoAutoAim(Target);

			if (canHitTarget(Target) && Armed) {
				Shoot();
			}
		} else {
			DoMouseAim();
		}

		float parentAngle = this.transform.parent.rotation.eulerAngles.y;

		Debug.DrawRay(this.transform.position, Quaternion.Euler(0, minAngle+parentAngle, 0) * Vector3.forward * 3);
		Debug.DrawRay(this.transform.position, Quaternion.Euler(0, maxAngle+parentAngle, 0) * Vector3.forward * 3);

		Debug.DrawRay(this.transform.position, Turret.transform.rotation * Vector3.forward * Turret.getAproxRange(), new Color(0.3f, 0, 0));
	}

	bool canHitTarget(GameObject target) {
		float turretAngle = Turret.transform.rotation.eulerAngles.y;

		Quaternion lookRotation = Quaternion.LookRotation(target.transform.position - this.transform.position);
		float angleToTarget = lookRotation.eulerAngles.y;

		float difference = Mathf.Abs(Mathf.DeltaAngle(turretAngle,angleToTarget));

		//Debug.Log (difference);

		if (difference < 5) return true;

		return false;
	}

	void DoAutoAim(GameObject target) {
		Quaternion lookRotation = Quaternion.LookRotation(target.transform.position - this.transform.position);
		float lookAngle = lookRotation.eulerAngles.y;
		
		// Current rotation of the ship object
		float parentAngle = this.transform.parent.rotation.eulerAngles.y;
		
		// Get the desired firing angle independent from current ship rotation
		float relativeDesiredAngle = lookAngle - parentAngle;
		if (relativeDesiredAngle > 180f) relativeDesiredAngle -= 360f; // ... and change it to -180° ... 180°
		if (relativeDesiredAngle < -180f) relativeDesiredAngle += 360f;
		
		float actualAngle = Utility.clampAngle(relativeDesiredAngle, minAngle, maxAngle) + parentAngle;
		
		//Debug.DrawRay(transform.position, Quaternion.Euler(0f, actualAngle, 0f) * Vector3.forward * 10f);
		
		Turret.transform.rotation = Quaternion.Euler(0f, actualAngle, 0f);
	}

	void DoMouseAim() {
		// Location of the mousePointer in the world (on plane y = 0f)
		Vector3 mousePointer = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
		
		// Quaternion of the rotation towards the mouse cursor relative from the turret location
		Quaternion lookRotation = Quaternion.LookRotation(mousePointer - Camera.main.transform.position - (this.transform.position - this.transform.parent.position));
		float lookAngle = lookRotation.eulerAngles.y;
		
		// Current rotation of the ship object
		float parentAngle = this.transform.parent.rotation.eulerAngles.y;
		
		// Get the desired firing angle independent from current ship rotation
		float relativeDesiredAngle = lookAngle - parentAngle;
		if (relativeDesiredAngle > 180f) relativeDesiredAngle -= 360f; // ... and change it to -180° ... 180°
		if (relativeDesiredAngle < -180f) relativeDesiredAngle += 360f;
		
		float actualAngle = Utility.clampAngle(relativeDesiredAngle, minAngle, maxAngle) + parentAngle;
		
		//Debug.DrawRay(transform.position, Quaternion.Euler(0f, actualAngle, 0f) * Vector3.forward * 10f);
		
		Turret.transform.rotation = Quaternion.Euler(0f, actualAngle, 0f);
	}
}
