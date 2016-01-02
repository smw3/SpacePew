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

	protected void turn(float direction) {
		myRigidbody.AddTorque(0f,direction,0f);
	}

    protected void turnTowards(float angle)
    {
        float currentAngle = this.transform.rotation.eulerAngles.y;

        float deltaAngle = Mathf.DeltaAngle(currentAngle, angle);
        Debug.Log("Turn towards: Delta angle: " + deltaAngle);

        if (deltaAngle > 0) turn(Mathf.Min(turnSpeed, deltaAngle));
        else turn(-Mathf.Min(turnSpeed, Mathf.Abs(deltaAngle)));

    }

	protected void toggleThrust(bool active) {
		isThrusting = active;

		if (active) {
			// TODO: Consider ship mass
			myRigidbody.velocity += transform.rotation * Vector3.forward * Acceleration;

			// Limit speed to maxspeed
			myRigidbody.velocity = myRigidbody.velocity.normalized * Mathf.Min(myRigidbody.velocity.magnitude, maxSpeed);
		}
	}
}
