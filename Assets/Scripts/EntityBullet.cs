using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;

[Serializable]
public class EntityCollisionEvent : UnityEvent<Collision>
{
}

public class EntityBullet : MonoBehaviour {
	[SerializeField]
	public EntityCollisionEvent Hit;
	public UnityEvent Fade;

	public float damage;

	public float TimeToDeath;

	public Transform ignoreObject;

	void Start () {

	}

	void Update () {
		float currentTime = Time.timeSinceLevelLoad;

		if (TimeToDeath <= currentTime) {
			Fade.Invoke();
			Destroy(this.gameObject);
		}
	}

	void OnCollisionEnter(Collision col) {
		if (col.gameObject.transform.IsChildOf(ignoreObject) || col.gameObject.transform == ignoreObject) return;

		Hit.Invoke(col);
		Destroy(this.gameObject);
	}
}
