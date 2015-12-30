using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public GameObject goTarget;

	// Camera Settings
	private float distance = 20f;


	// Caching
	private Transform tTarget_transform;
	private Transform tCamera_transform;

	// GameObjects to be Parallax scrolled
	public Transform[] tParallax_Objects;
	public float[] fParallax_Amount; // 1 = no movement. 0 = full movement

	void Start () {
		tTarget_transform = goTarget.transform;
		tCamera_transform = this.transform;
	}

	void Update () {
		tCamera_transform.position = tTarget_transform.position + new Vector3(0f, distance, 0f);

		for (int i=0; i < tParallax_Objects.Length; i++) {
			Vector3 v3ParallaxPosition = new Vector3(
				this.transform.position.x * fParallax_Amount[i],
				tParallax_Objects[i].position.y,
				this.transform.position.z * fParallax_Amount[i]
				);
			tParallax_Objects[i].position = v3ParallaxPosition;
		}
	}
}
