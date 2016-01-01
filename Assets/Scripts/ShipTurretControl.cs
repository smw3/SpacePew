using UnityEngine;
using System.Collections;

public class ShipTurretControl : MonoBehaviour {

	public GameObject Turret;

	// List of all available turret mounts on this ship object
	private ArrayList talTurretMounts = new ArrayList();

	void Start () {
		for (int i=0; i < this.transform.childCount; i++) {
			Transform child = this.transform.GetChild(i);

			TurretMount turretMount = child.GetComponent<TurretMount>();
			if (turretMount) {
				talTurretMounts.Add (turretMount);
			}
		}

		foreach(TurretMount mount in talTurretMounts) {
			mount.Populate();
		}
	}

	public void Shoot() {
		foreach(TurretMount mount in talTurretMounts) {
			mount.manualShoot();
		}
	}

	void FixedUpdate () {
		if (Input.GetMouseButton(0)) {
			Shoot ();
		}
	}
}
