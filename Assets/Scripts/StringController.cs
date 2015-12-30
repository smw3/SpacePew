using System;


public class StringController
{
	// Singleton Instance
	public static StringController instance = null;

	// String assignments
	public readonly string Resource_Turret_DebugTurret = "DebugTurret"; // The name of the basic Turret prefab

	public readonly string Resource_MainThruster_DebugMainThruster = "DebugMainThruster";

	public readonly string Tag_EntityProjectile = "EntityProjectile";

	// Singleton functionality
	public static StringController GetInstance() {
		if (instance == null) instance = new StringController();
		return instance;
	}
}


