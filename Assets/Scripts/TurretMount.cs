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

		if (Automated && Target && Armed) {
            DoAutoAim(Target);

            if (canHitTarget(Target) && Armed)
            {
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

	public bool canHitTarget(GameObject target) {
        Collider[] targetColliders = target.GetComponentsInChildren<Collider>();

        Ray myRay = new Ray(this.transform.position, Turret.transform.rotation * Vector3.forward);
        RaycastHit[] hits = Physics.RaycastAll(myRay, Turret.getAproxRange());

        //Debug.Log("Ray at angle " + (idealAngle + a));

        foreach (RaycastHit hit in hits)
        {
            foreach (Collider col in targetColliders)
            {
                if (hit.collider == col)
                {
                    //Debug.Log("Ray hit the collider with angle " + a);
                    return true;
                }
            }
        }

        return false;
    }

	void DoAutoAim(GameObject target) {
		Quaternion lookRotation = Quaternion.LookRotation(target.transform.position - this.transform.position);
		float targetAngle = lookRotation.eulerAngles.y;
        Collider[] targetColliders = target.GetComponentsInChildren<Collider>();

        // Current rotation of the ship object
        float parentAngle = this.transform.parent.rotation.eulerAngles.y;

        float idealAngle = minAngle + (maxAngle - minAngle) / 2 + parentAngle;
        while (idealAngle < 0.0f) idealAngle += 360.0f;
        while (idealAngle > 360.0f) idealAngle -= 360.0f;


        float step = 2.0f;

        //Debug.Log("Target is at " + targetAngle + " but " + idealAngle + " would be better");


        // Ideally, a turret wants to shoot at the angle where it has the maximum amount of movement left, i.e. its middle
        // Cast a ray in its middle, see if it hits.
        // Continue casting rays every [step] degrees until one hits, continuously getting further away from the ideal angle
        // Use the first one, i.e. the clostest one to the middle

        bool hasHit = false;
        for (float a = 0; a < (maxAngle - minAngle) / 2; a += step)
        {
            for (float sign = -1f; sign < 1; sign += 2f)
            { 
                Ray myRay = new Ray(this.transform.position, Quaternion.Euler(0f, idealAngle + a*sign, 0f) * Vector3.forward);
                RaycastHit[] hits = Physics.RaycastAll(myRay, Turret.getAproxRange());

                //Debug.Log("Ray at angle " + (idealAngle + a));

                foreach (RaycastHit hit in hits)
                {
                    foreach (Collider col in targetColliders)
                    {
                        if (hit.collider == col)
                        {
                            //Debug.Log("Ray hit the collider with angle " + a);
                            hasHit = true;
                            break;
                        }
                    }
                }

                if (hasHit)
                {
                    targetAngle = idealAngle + a * sign;
                    break;
                }
            }

            if (hasHit) break;
        }

        //Debug.Log("Final angle: " + targetAngle);

        // Get the desired firing angle independent from current ship rotation
        float relativeDesiredAngle = targetAngle - parentAngle;
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
