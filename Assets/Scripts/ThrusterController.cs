using UnityEngine;
using System.Collections;

public class ThrusterController : MonoBehaviour {

	ParticleSystem PS;

	void Start () {
		PS = GetComponent<ParticleSystem>();
	}
	
	public void Enable() {
		if (!PS) return;

		PS.enableEmission = true;
	}

	public void Disable() {
		if (!PS) return;

		PS.enableEmission = false;
	}

	void Update () {
	
	}
}
