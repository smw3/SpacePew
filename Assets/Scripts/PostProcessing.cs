using UnityEngine;
using System.Collections;

public class PostProcessing : MonoBehaviour {

	public Shader compositeShader;

	private Material compositeMaterial;
	public Material CompositeMaterial {
		get {
			if (compositeMaterial == null) {
				compositeMaterial = new Material(compositeShader);
				compositeMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return compositeMaterial;
		}
	}

	private RenderTexture renderTexture;
	public GameObject shaderCamera_Glow;

	void OnDisable() {
		DestroyImmediate(compositeMaterial, true);

		if (renderTexture != null) {
			RenderTexture.ReleaseTemporary(renderTexture);
			renderTexture = null;
		}
	}

	void Start () {
		if (!SystemInfo.supportsImageEffects) {
			Debug.LogWarning("Image Effects not supported! Post Processing will NOT work! Deactivating");
			enabled = false;
			return;
		}

		shaderCamera_Glow.GetComponent<Camera>().enabled = false;
	}
	
	void OnPreRender() {
		if (!enabled || !gameObject.activeInHierarchy)
			return;

		if (renderTexture != null) {
			RenderTexture.ReleaseTemporary(renderTexture);
			renderTexture = null;
		}
		renderTexture = RenderTexture.GetTemporary(GetComponent<Camera>().pixelWidth, GetComponent<Camera>().pixelHeight, 16);

		Camera cam = shaderCamera_Glow.GetComponent<Camera>();

		int oldCullMask = cam.cullingMask;

		cam.CopyFrom(GetComponent<Camera>());
		cam.cullingMask = oldCullMask;
		cam.backgroundColor = new Color(0,0,0,0);
		cam.clearFlags = CameraClearFlags.SolidColor;
		cam.targetTexture = renderTexture;
		cam.Render();
	}

	void OnRenderImage(RenderTexture source, RenderTexture destination) {
		CompositeMaterial.SetTexture("_GlowTex", renderTexture);


		Graphics.Blit(source, destination, CompositeMaterial);

		if (renderTexture != null) {
			RenderTexture.ReleaseTemporary(renderTexture);
			renderTexture = null;
		}
	}
}
