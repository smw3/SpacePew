using UnityEngine;
using System.Collections;

public class AutoShipMovementControl : ShipMovementControl {

    public GameObject Target;
    public float desiredDistance;

    public bool active;

    private float currentTargetAngle;

    void Update () {
        if (!active) return;
        if (!Target) return;

        turnTowards(currentTargetAngle);

        if ((Time.frameCount % 120) != 0) return;
        // Only do pathfinding every couple of frames

        float currentAngleFromTarget = Quaternion.LookRotation(this.transform.position - Target.transform.position).eulerAngles.y;
        Debug.Log(currentAngleFromTarget);

        float currentDistanceToTarget = Vector3.Magnitude(this.transform.position - Target.transform.position);

        
        // Find the tangent point of a circle around the target with radius [desiredDistance]
        // In essence, this should make the ship want to "orbit" its target

        float hypo = currentDistanceToTarget;
        float opposite = desiredDistance;

        if (hypo < opposite) hypo = opposite;

        float angleToTangent = Mathf.Asin(opposite / hypo) * Mathf.Rad2Deg;
        float distanceToTangent = Mathf.Sqrt(hypo * hypo - opposite * opposite);

        //Debug.Log("Angle: " + angleToTangent + " distance: " + distanceToTangent);

        Vector3 clockwiseTangentPoint = this.Target.transform.position + Quaternion.Euler(0f, angleToTangent + currentAngleFromTarget, 0f) * Vector3.forward * desiredDistance;
        Vector3 antiClockwiseTangentPoint = this.Target.transform.position + Quaternion.Euler(0f, -angleToTangent + currentAngleFromTarget, 0f) * Vector3.forward * desiredDistance;

        float distanceToClockwise = Vector3.SqrMagnitude(clockwiseTangentPoint - (this.transform.position + this.transform.rotation * Vector3.forward));
        float distanceToAntiClockwise = Vector3.SqrMagnitude(antiClockwiseTangentPoint - (this.transform.position + this.transform.rotation * Vector3.forward));

        Debug.DrawRay((this.transform.position + this.transform.rotation * Vector3.forward * 5f) + Vector3.left * 0.5f, Vector3.right, Color.blue);
        Debug.DrawRay((this.transform.position + this.transform.rotation * Vector3.forward * 5f) + Vector3.back * 0.5f, Vector3.forward, Color.blue);

        Vector3 targetPoint;

        if (distanceToClockwise < distanceToAntiClockwise)
        {
            //Debug.Log("Fly towards clockwise (red)!");
            targetPoint = clockwiseTangentPoint;
        } else {
            //Debug.Log("Fly towards anti-clockwise (green)");
            targetPoint = antiClockwiseTangentPoint;
        }

        currentTargetAngle = Quaternion.LookRotation(this.transform.position - targetPoint).eulerAngles.y + 180f;
        toggleThrust(true);


        Debug.DrawRay(clockwiseTangentPoint + Vector3.left * 0.5f, Vector3.right, Color.red);
        Debug.DrawRay(clockwiseTangentPoint + Vector3.back * 0.5f, Vector3.forward, Color.red);


        Debug.DrawRay(antiClockwiseTangentPoint + Vector3.left * 0.5f, Vector3.right, Color.green);
        Debug.DrawRay(antiClockwiseTangentPoint + Vector3.back * 0.5f, Vector3.forward, Color.green);
    }
}
