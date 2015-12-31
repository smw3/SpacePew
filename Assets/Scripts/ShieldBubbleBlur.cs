using UnityEngine;
using System.Collections;

public class ShieldBubbleBlur : MonoBehaviour {
	
	private RenderTexture rtBuffer1;
    private RenderTexture rtBuffer2;

    private RenderTexture currentBuffer;

	private Material shieldMaterial;
	private MeshRenderer MR;
	private Mesh mesh;

	private Camera cam;

	void Start () {
		MR = this.transform.parent.GetComponent<MeshRenderer>();
		mesh = this.transform.parent.GetComponent<MeshFilter>().mesh;
		
		shieldMaterial = MR.material;

		cam = transform.GetComponent<Camera>();

		cam.transform.localPosition = new Vector3(0,5.0f,0);
		cam.transform.rotation = Quaternion.Euler(90, 0, 0);
		
		cam.orthographic = true;
		
		cam.nearClipPlane = 0.3f;
		cam.farClipPlane = 100.0f;
		
		//cam.backgroundColor = new Color(0,0,0,1);
		//cam.clearFlags = CameraClearFlags.SolidColor;

		cam.orthographicSize = mesh.bounds.extents.z * this.transform.parent.localScale.z;

		Debug.Log (mesh.bounds.extents.z+" "+this.transform.parent.localScale.z);

		rtBuffer1 = new RenderTexture(Mathf.NextPowerOfTwo(cam.pixelWidth), Mathf.NextPowerOfTwo(cam.pixelHeight), 24, RenderTextureFormat.ARGB32);
		rtBuffer2 = new RenderTexture(Mathf.NextPowerOfTwo(cam.pixelWidth), Mathf.NextPowerOfTwo(cam.pixelHeight), 24, RenderTextureFormat.ARGB32);
		rtBuffer1.Create();
		rtBuffer2.Create();

		cam.targetTexture = rtBuffer1;
	}
	
	void OnPreRender() {
		//RenderTexture old = RenderTexture.active;
		//RenderTexture.active = rtBuffer1;

		//cam.transform.rotation = Quaternion.Euler(90, 0, 0);
		cam.orthographicSize = mesh.bounds.extents.z * this.transform.parent.localScale.z;
		//cam.transform.position = this.transform.parent.position + Vector3.up*50.0f;
		//cam.transform.rotation = Quaternion.Euler(this.transform.parent.rotation.x + 90, this.transform.parent.rotation.y, this.transform.parent.rotation.z);

		if ((Time.frameCount % 2 == 0)) {
			currentBuffer = rtBuffer2;
			cam.targetTexture = rtBuffer1;
		} else {
			currentBuffer = rtBuffer1;
			cam.targetTexture = rtBuffer2;
		}

		shieldMaterial.SetTexture("_LastFrame", currentBuffer);

		float ratio = (mesh.bounds.extents.x * this.transform.parent.localScale.x) / (cam.aspect * cam.orthographicSize);

		float TextureScaleY = (mesh.bounds.extents.z * this.transform.parent.localScale.z) / cam.orthographicSize;
		float TextureScaleX = ratio;
		
		//Debug.Log (TextureScaleX+" "+TextureScaleY);
		
		shieldMaterial.SetTextureScale("_LastFrame", new Vector2(TextureScaleX, TextureScaleY));
		
		//RenderTexture.active = old;
	}
}
