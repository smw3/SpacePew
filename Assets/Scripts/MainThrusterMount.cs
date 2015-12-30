using UnityEngine;
using System.Collections;

public class MainThrusterMount : MonoBehaviour {
		
	private GameObject Thruster;

	private ShipMovementControl shipMovementControl;

	void Start () {
		shipMovementControl = transform.parent.GetComponent<ShipMovementControl>();
	}
	
	public void Populate ()
	{
		Thruster = (GameObject)Instantiate(Resources.Load (StringController.GetInstance().Resource_MainThruster_DebugMainThruster));
		
		Thruster.transform.parent = this.transform;
		Thruster.transform.localPosition = Vector3.zero;
	}
	
	public void EnableThrustEffect()
	{
		if (Thruster) {
			Thruster.GetComponent<ThrusterController>().Enable();
		}
	}

	public void DisableThrustEffect() {
		if (Thruster) {
			Thruster.GetComponent<ThrusterController>().Disable();
		}
	}
	
	void Update () {
		if (shipMovementControl.isThrusting) {
			EnableThrustEffect();
		} else {
			DisableThrustEffect();
		}
	}

}
