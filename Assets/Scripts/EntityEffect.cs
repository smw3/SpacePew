using UnityEngine;
using System.Collections;

public class EntityEffect : MonoBehaviour {
	public float CreationTime;
	public float TimeToLive;

	void Start() {
		CreationTime = Time.timeSinceLevelLoad;
	}

	void Update() {
		//Debug.DrawRay(this.transform.position, Vector3.left);
		//Debug.DrawRay(this.transform.position, Vector3.back);

		if ((CreationTime + TimeToLive) <= Time.timeSinceLevelLoad) Destroy (this.gameObject);
	}

	public float getFade() {
		return ((CreationTime + TimeToLive) - Time.timeSinceLevelLoad) / TimeToLive;
	}

}
