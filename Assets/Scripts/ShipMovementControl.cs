using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class ShipMovementControl : MonoBehaviour {
	private Rigidbody myRigidbody = null;

	// List of all available turret mounts on this ship object
	private ArrayList MainThrusterMounts = new ArrayList();

	public bool isThrusting = false;

	public float maxSpeed = 30f;
	public float Acceleration = .5f;

	public float turnSpeed = 16f;

	void Start() {
		myRigidbody = GetComponent<Rigidbody>();

		for (int i=0; i < this.transform.childCount; i++) {
			Transform child = this.transform.GetChild(i);

			MainThrusterMount mainThrusterMount = child.GetComponent<MainThrusterMount>();
			if (mainThrusterMount) {
				MainThrusterMounts.Add (mainThrusterMount);
			}
		}
		
		foreach(MainThrusterMount mount in MainThrusterMounts) {
			mount.Populate();
		}
	}

	void FixedUpdate() {
		if (Input.GetKey(KeyCode.A)) {
			turn (-turnSpeed);
		} else if (Input.GetKey(KeyCode.D)) {
			turn (turnSpeed);
		}

		if (Input.GetKey (KeyCode.W)) {
			toggleThrust(true);
		} else {
			toggleThrust(false);
		}

		//Debug.Log (myRigidbody.velocity.magnitude+" "+Time.frameCount);
	}

	private void turn(float direction) {
		myRigidbody.AddTorque(0f,direction,0f);
	}

	private void toggleThrust(bool active) {
		isThrusting = active;

		if (active) {
			// TODO: Consider ship mass
			myRigidbody.velocity += transform.rotation * Vector3.forward * Acceleration;

			// Limit speed to maxspeed
			myRigidbody.velocity = myRigidbody.velocity.normalized * Mathf.Min(myRigidbody.velocity.magnitude, maxSpeed);
		}
	}
}
