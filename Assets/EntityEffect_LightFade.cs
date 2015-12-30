using UnityEngine;
using System.Collections;

[RequireComponent (typeof (EntityEffect))]
[RequireComponent (typeof (Light))]
public class EntityEffect_LightFade : MonoBehaviour {

	private EntityEffect effectController;
	private Light effectLight;

	private float intensity;

	void Start () {
		effectController = GetComponent<EntityEffect>();
		effectLight = GetComponent<Light>();
	}

	public void setIntensity(float amt) {
		intensity = amt;
	}
	
	void Update () {
		effectLight.intensity = intensity * effectController.getFade();
	}
}
