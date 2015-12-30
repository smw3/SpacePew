using UnityEngine;
using System.Collections;

public class StarfieldController : MonoBehaviour {
	private class Star {
		public GameObject goStar;
		public Transform tStar;

		public Vector2 v2Offset;
		public float fParallaxAmount;
	}

	// Settings set in the Editor
	public GameObject goStarPrefab;
	public int iStarAmount;

	// List of stars in our starfield
	private ArrayList alStarfield = new ArrayList();

	// Cache the area in world space that the camera is looking at
	private float fResolutionX;
	private float fResolutionY;
	// Cache the camera
	private Transform tCamera;

	void Start () {
		// Calculate screen space to world space area
		fResolutionX = Camera.main.GetComponent<Camera>().orthographicSize;
		fResolutionY = fResolutionX * Screen.width / Screen.height;

		// Cache the camera's transform
		tCamera = Camera.main.transform;

		//Debug.Log(fResolutionX+" "+fResolutionY);

		Quaternion qStarRotation = Quaternion.Euler(new Vector3(90f,0f,0f));

		for (int i=0; i < iStarAmount; i++) {
			Star myNewStar = new Star();

			GameObject goNewStarObject = (GameObject)Instantiate(goStarPrefab, new Vector3(0F, 0F, 0F), qStarRotation);

			goNewStarObject.transform.parent = this.transform; // Make the newly created GameObject a parent of the Starfield to avoid clutter in the Hierachy

			myNewStar.goStar = goNewStarObject;
			myNewStar.tStar = goNewStarObject.transform;

			float size = Random.Range(0.01f, 0.1f);
			myNewStar.tStar.localScale = new Vector3(size,size,size);

			myNewStar.fParallaxAmount = Random.Range(0.7f,1f); // 1 = infinite distance, 0 would be same plane as nearby objects, which makes no sense for stars
			myNewStar.v2Offset = new Vector2(
				Random.Range(-fResolutionX, fResolutionX),
				Random.Range (-fResolutionY, fResolutionY)
				);

			alStarfield.Add(myNewStar);
		}		                                  
	}

	void Update () {
		Vector3 v3CameraPosition = tCamera.position;

		foreach(Star myStar in alStarfield) {

			float fOffsetX =  v3CameraPosition.x * myStar.fParallaxAmount;
			float fPositionX = myStar.v2Offset.x + fOffsetX;


			while (fPositionX < (v3CameraPosition.x - fResolutionX)) {
				fPositionX += fResolutionX*2;
			}
			while (fPositionX > (v3CameraPosition.x + fResolutionX)) {
				fPositionX -= fResolutionX*2;
			}

			float fOffsetY = v3CameraPosition.z * myStar.fParallaxAmount;
			float fPositionY = myStar.v2Offset.y + fOffsetY;
			
			
			while (fPositionY < (v3CameraPosition.z - fResolutionY)) {
				fPositionY += fResolutionY*2;
			}
			while (fPositionY > (v3CameraPosition.z + fResolutionY)) {
				fPositionY -= fResolutionY*2;
			}

			myStar.tStar.position = new Vector3(fPositionX, myStar.fParallaxAmount * -10f, fPositionY);
		}

	}
}
