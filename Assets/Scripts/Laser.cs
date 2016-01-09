using UnityEngine;

public class Laser : MonoBehaviour {

    public float maxLength;
    public float currentLength;
    public float width;

    public float damage;

    public float TimeAtCreation;
    private float Lifetime;

    public float DamageInterval;
    private float lastDamageInterval = 0;

    public Material laserMaterial;
    private Color defaultColor;

    private Vector3[] vertices;
    private Vector2[] UVs;
    private Vector2[] UV2s;
    private int[] triangles;

    void Start() {

        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        Ray laserRay = new Ray(transform.position, fwd);
        RaycastHit hit;

        currentLength = maxLength;

        if (Physics.Raycast(laserRay, out hit, maxLength)) {
            //Debug.Log (hit.transform.name+" was lasered");
            currentLength = hit.distance - 2.2f;

            ILaserTarget laserTarget = hit.collider.gameObject.GetComponent<ILaserTarget>();
            if (laserTarget != null) {
                laserTarget.OnLaserHit(hit.point, damage);
            }
        }

        vertices = createVertices(width, currentLength);
        triangles = createTriangles();
        UVs = createUVs();
        UV2s = createUV2s();

        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        mesh.Clear();

        updateVertices(width, currentLength);
        mesh.uv = UVs;
        mesh.uv2 = UV2s;
        mesh.SetTriangles(triangles, 0);

        meshRenderer.material = laserMaterial;
        defaultColor = laserMaterial.GetColor("_TintColor");

        Lifetime = 2f;
        TimeAtCreation = Time.timeSinceLevelLoad;
    }

    void FixedUpdate() {
        // If the object's lifetime is over, destroy
        if (TimeAtCreation + Lifetime <= Time.timeSinceLevelLoad) {
            Destroy(this.gameObject);
            return;
        }

        float fade = 1 - (Time.timeSinceLevelLoad - TimeAtCreation) / Lifetime;

        this.laserMaterial.SetColor("_TintColor", new Color(defaultColor.r, defaultColor.b, defaultColor.g, fade));

        currentLength = maxLength;

        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        Ray laserRay = new Ray(transform.position, fwd);
        RaycastHit hit;

        if (Physics.Raycast(laserRay, out hit, maxLength)) {
            currentLength = hit.distance - 2.2f;

            ILaserTarget laserTarget = hit.collider.gameObject.GetComponent<ILaserTarget>();
            if (laserTarget != null && canDamage()) {
                laserTarget.OnLaserHit(hit.point, damage);
            }
        }

        updateVertices(width, currentLength);

    }

    private bool canDamage() {

        // update vertices
        // Check if the last shot was long enough ago
        if (lastDamageInterval + DamageInterval < Time.timeSinceLevelLoad) {
            // We are going to fire. Save the time
            lastDamageInterval = Time.timeSinceLevelLoad;
            return true;
        }
        return false;
    }

    void updateVertices(float newWidth, float newLength) {
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        vertices = createVertices(newWidth, newLength);
        mesh.vertices = vertices;
    }

    // MESH GENERATION
    // ---------------

    // UVs for the laser texture, ranging from 0..1 on both axis, making sure that the laser texture does not repeat
    private Vector2[] createUVs() {
        // 0	1	2	3
        // 
        //
        // 4	5	6	7
        Vector2[] newUVs = new Vector2[8];

        newUVs[0] = new Vector2(0f, 0f);
        newUVs[1] = new Vector2(0.33f, 0f);
        newUVs[4] = new Vector2(0f, 1f);
        newUVs[5] = new Vector2(0.33f, 1f);

        newUVs[2] = new Vector2(0.66f, 0f);
        newUVs[3] = new Vector2(1f, 0f);
        newUVs[6] = new Vector2(0.66f, 1f);
        newUVs[7] = new Vector2(1f, 1f);

        return newUVs;
    }

    // UVs for the interference (aka detail) texture, which should NOT be scaled to fit the mesh, but repeated, hence the UVs go from 0...whatever
    private Vector2[] createUV2s() {
        // 0	1	2	3
        // 
        //
        // 4	5	6	7
        Vector2[] newUVs = new Vector2[8];

        for (int i = 0; i < 8; i++) {
            newUVs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        return newUVs;
    }

    // 3 distinct rectangles:
    // 0145 is the laser beginning, which is always 1f long
    // 1256 is the laser middle, which has a dynamic length
    // 1267 is the laser end, which is always 1f long
    private Vector3[] createVertices(float laserWidth, float laserMiddleLength) {
        // 0	1	2	3
        // 
        //
        // 4	5	6	7

        Vector3[] newVertices = new Vector3[8];

        newVertices[0] = new Vector3(0f, 0f, 0f);
        newVertices[1] = new Vector3(0f, 0f, 1f);
        newVertices[4] = new Vector3(laserWidth, 0f, 0f);
        newVertices[5] = new Vector3(laserWidth, 0f, 1f);

        newVertices[2] = new Vector3(0f, 0f, 1f + laserMiddleLength);
        newVertices[3] = new Vector3(0f, 0f, 2f + laserMiddleLength);
        newVertices[6] = new Vector3(laserWidth, 0f, 1f + laserMiddleLength);
        newVertices[7] = new Vector3(laserWidth, 0f, 2f + laserMiddleLength);

        return newVertices;
    }

    private int[] createTriangles() {
        // 0	1	2	3
        // 
        //
        // 4	5	6	7

        return new int[] { 0, 5, 4, 0, 1, 5, 1, 6, 5, 1, 2, 6, 2, 7, 6, 2, 3, 7 };
    }
}
