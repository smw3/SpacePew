using UnityEngine;
using System.Collections;

public class EntityShieldController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnHit (EntityBullet entityBullet)
	{
		Debug.Log ("Foobar");
	}
}
