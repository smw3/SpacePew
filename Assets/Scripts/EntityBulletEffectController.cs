using UnityEngine;
using System.Collections;

public class EntityBulletEffectController : MonoBehaviour {

	public EntityEffect OnHitEffect;
	public EntityEffect OnFadeEffect;

	public void OnHit(Collision col) {
		var newEffect = (EntityEffect)Instantiate(OnHitEffect, this.transform.position, Quaternion.identity);
		this.transform.parent = newEffect.transform;
	}

	public void OnFade() {
		var newEffect = (EntityEffect)Instantiate(OnFadeEffect, this.transform.position, Quaternion.identity);
		this.transform.parent = newEffect.transform;
	}
}
