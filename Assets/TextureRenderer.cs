using UnityEngine;
using System.Collections;

public class TextureRenderer : MonoBehaviour {

	private Camera cam;
	private RenderTexture renderTexture;

	private Transform quad;

	public void setMaterial(Material mat) {
		quad = transform.Find("Quad");

		quad.GetComponent<MeshRenderer>().material = mat;
	}

	void OnPreRender() {

		if (renderTexture != null) {
			RenderTexture.ReleaseTemporary(renderTexture);
			renderTexture = null;
		}
		renderTexture = RenderTexture.GetTemporary(GetComponent<Camera>().pixelWidth, GetComponent<Camera>().pixelHeight, 16);

		cam = GetComponent<Camera>();

		cam.backgroundColor = new Color(0,0,0,0);
		cam.clearFlags = CameraClearFlags.SolidColor;
		cam.targetTexture = renderTexture;
	
		//cam.Render();
	}

	void OnRenderImage(RenderTexture source, RenderTexture destination) {
		Debug.Log ("Render image!");
		Graphics.Blit(source, destination);
	}

	public Texture getTexture() {
		if (renderTexture != null) {
			Debug.Log ("Render texture available!");
			return (Texture)renderTexture;
		}
		return Texture2D.blackTexture;
	}


}
