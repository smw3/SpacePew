using UnityEngine;
using System.Collections;

public class ManualShipMovementControl : ShipMovementControl {

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            turn(-turnSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            turn(turnSpeed);
        }

        if (Input.GetKey(KeyCode.W))
        {
            toggleThrust(true);
        }
        else
        {
            toggleThrust(false);
        }
    }
}
