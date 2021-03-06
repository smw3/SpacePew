//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using UnityEngine;

public class Utility
{
	public static float clampAngle(float angle, float leftClamp, float rightClamp) {
		if (angle < leftClamp) return leftClamp;
		if (angle > rightClamp) return rightClamp;
		return angle;
	}
    
    public static bool checkIdealFiringAngleForTarget(Ray leftBound, Ray center, Ray rightBound, GameObject target, float range) {
        // Check if center ray hits

        Collider[] targetColliders = target.GetComponentsInChildren<Collider>();

        bool hasHit = false;
        RaycastHit[] hits = Physics.RaycastAll(center, range);
        foreach (RaycastHit hit in hits) {
            foreach (Collider col in targetColliders) {
                if (hit.collider == col) {
                    hasHit = true;
                    break;
                }
            }
        }

        if (!hasHit) return false; // If center ray misses, the angle can't be great

        // check left bound
        hasHit = false;
        hits = Physics.RaycastAll(leftBound, range);
        foreach (RaycastHit hit in hits) {
            foreach (Collider col in targetColliders) {
                if (hit.collider == col) {
                    hasHit = true;
                    break;
                }
            }
        }

        if (!hasHit) return false; // If left ray missed, the angle can't be great

        // check right bound
        hasHit = false;
        hits = Physics.RaycastAll(rightBound, range);
        foreach (RaycastHit hit in hits) {
            foreach (Collider col in targetColliders) {
                if (hit.collider == col) {
                    hasHit = true;
                    break;
                }
            }
        }

        return hasHit;
    }
}

