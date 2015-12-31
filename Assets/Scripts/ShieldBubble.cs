using UnityEngine;
using System.Collections;

public class ShieldBubble : MonoBehaviour, ILaserTarget {

	private MeshRenderer MR;
	private Color defaultColor;

	public float FadeAmount = .05f;

	public EntityEffect OnHitEffect;

	private string MaterialColor = "_TintColor";

    private int index = 0;
    private Matrix4x4 hitDataMatrix = new Matrix4x4();

    private struct shieldHit
	{
		public Vector3 pos;
		public float time;
		public float intensity;
	}
	private Queue hitQueue = new Queue();

	void Start () {
		MR = GetComponent<MeshRenderer>();
		defaultColor = MR.material.GetColor(MaterialColor);
    }

	public void OnLaserHit(Vector3 collisionPoint, float damage) {
		ShieldHit(collisionPoint, damage);
	}

	void OnCollisionEnter(Collision col) {
		float damage = 10f;

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

	void OnRenderObject() {
        shieldHit sh;
        for (int i=0; i<4; i++)
        {
            if (hitQueue.Count == 0) break;
            sh = (shieldHit)hitQueue.Dequeue();

            // Discard any hit longer than 2 seconds ago
            if ((Time.timeSinceLevelLoad - sh.time) > 2.0f) break;

            hitDataMatrix.SetRow(index, new Vector4(sh.pos.x, sh.pos.y, sh.pos.z, sh.time));

            index = (index + 1) % 4;
        }


        MR.material.SetMatrix("_ShieldHitBuffer", hitDataMatrix);
	}

	private void ShieldHit(Vector3 contactPoint, float intensity) {
		shieldHit sh = new shieldHit();

		sh.pos = contactPoint;
		sh.time = Time.timeSinceLevelLoad;
		sh.intensity = intensity;

		hitQueue.Enqueue(sh);

		EntityEffect LightEffect = (EntityEffect)Instantiate(OnHitEffect, contactPoint, Quaternion.identity);
		LightEffect.GetComponent<EntityEffect_LightFade>().setIntensity(intensity);
	}

    public Queue getImpactLocationsQueue() {
        return hitQueue;

    }

	private void SetShieldIntensity(float intensity) {
		float maxAlpha = defaultColor.a;

		MR.material.SetColor(MaterialColor, new Color(defaultColor.r, defaultColor.b, defaultColor.g, intensity*maxAlpha));
	}
}
