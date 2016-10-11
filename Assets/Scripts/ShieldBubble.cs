using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShieldBubble : MonoBehaviour, ILaserTarget {

	private MeshRenderer MR;

	public EntityEffect OnHitEffect;

    private int index = 0;
    private Matrix4x4 hitDataMatrix = new Matrix4x4();

    private struct shieldHit
	{
		public Vector3 pos;
		public float time;
		public float intensity;
	}

    private List<shieldHit> shieldHitArray = new List<shieldHit>();

	void Start () {
		MR = GetComponent<MeshRenderer>();
    }

	public void OnLaserHit(Vector3 collisionPoint, float damage) {
		ShieldHit(collisionPoint, damage);
	}

	void OnCollisionEnter(Collision col) {
		float damage = 10f; // CHANGE ME

		if (col.gameObject.GetComponent<EntityBullet>()) {
			EntityBullet bullet = (EntityBullet)col.gameObject.GetComponent<EntityBullet>();

			if (bullet.ignoreObject == this.transform || this.transform.IsChildOf(bullet.ignoreObject)) return;

			damage = bullet.damage;

			//Debug.Log ("Collided with bullet");
		} else {
			//Debug.Log ("Something else");
		}

		ShieldHit(col.contacts[0].point, damage);
	}

    void FixedUpdate() {
        Debug.Log(shieldHitArray.Count);

        // Weed out shieldHits that are expired
        float current_time = Time.timeSinceLevelLoad;
        float expiration_time = MR.material.GetFloat("_RippleFadeTime");
        List <shieldHit> toDelete = new List<shieldHit>();

        foreach (shieldHit sh in shieldHitArray) {
            if ((current_time - sh.time) > expiration_time) {
                toDelete.Add(sh);
            }
        }

        foreach (shieldHit sh in toDelete) {
            shieldHitArray.Remove(sh);
        }
    }

	void OnRenderObject() {

        int points_length = shieldHitArray.Count;

        MR.material.SetInt("_Points_Length", points_length);
        for (int i = 0; i < points_length; i++) {
            shieldHit sh = shieldHitArray[i];
            MR.material.SetVector("_Points" + i.ToString(), sh.pos);

            Vector3 properties = new Vector3(sh.intensity * 5, sh.intensity, sh.time);
            MR.material.SetVector("_Properties" + i.ToString(), properties);
        }
	}

	private void ShieldHit(Vector3 contactPoint, float intensity) {
		shieldHit sh = new shieldHit();

		sh.pos = contactPoint;
		sh.time = Time.timeSinceLevelLoad;
		sh.intensity = intensity;

        shieldHitArray.Add(sh);

        //EntityEffect LightEffect = (EntityEffect)Instantiate(OnHitEffect, contactPoint, Quaternion.identity);
        //LightEffect.GetComponent<EntityEffect_LightFade>().setIntensity(intensity);
    }
}
