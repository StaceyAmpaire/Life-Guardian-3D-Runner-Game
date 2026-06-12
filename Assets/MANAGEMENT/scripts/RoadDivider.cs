using UnityEngine;

/// <summary>
/// RoadDivider - White dashed center line for your road.
/// Road position: (400, 0, 0) | Scale: (10, 0.1, 3000)
/// </summary>
public class RoadDivider : MonoBehaviour
{
    [Header("Dash Settings")]
    public float dashLength = 2f;
    public float gapLength  = 4f;
    public float dashWidth  = 0.25f;

    [Header("Material")]
    public Material dashMaterial;

    private const float ROAD_X       = 400f;
    private const float ROAD_Y       = 0f;
    private const float ROAD_Z       = 0f;
    private const float ROAD_SCALE_Z = 3000f;
    private const float ROAD_SCALE_Y = 0.1f;

    private GameObject _dashParent;

    void Start() => GenerateDivider();

    [ContextMenu("Generate In Editor")]
    public void GenerateDivider()
    {
        if (_dashParent != null)
            DestroyImmediate(_dashParent);

        _dashParent = new GameObject("Dashes");
        _dashParent.transform.SetParent(transform);
        _dashParent.transform.localPosition = Vector3.zero;

        if (dashMaterial == null)
        {
            // Use Unlit/Color so it is ALWAYS bright white — no lighting needed
            dashMaterial = new Material(Shader.Find("Unlit/Color"));
            dashMaterial.color = Color.white;
        }

        float roadSurfaceY = ROAD_Y + (ROAD_SCALE_Y / 2f) + 0.012f; // just above surface
        float roadStart    = ROAD_Z - ROAD_SCALE_Z / 2f;             // -1500
        float roadEnd      = ROAD_Z + ROAD_SCALE_Z / 2f;             //  1500

        float step     = dashLength + gapLength;
        float currentZ = roadStart;
        int   index    = 0;

        while (currentZ + dashLength <= roadEnd)
        {
            GameObject dash = GameObject.CreatePrimitive(PrimitiveType.Cube);
            dash.name = $"Dash_{index}";
            dash.transform.SetParent(_dashParent.transform);
            dash.transform.localScale = new Vector3(dashWidth, 0.001f, dashLength); // FLAT - almost zero height

            dash.transform.position = new Vector3(
                ROAD_X,
                roadSurfaceY,
                currentZ + dashLength * 0.5f
            );

            // Disable shadows completely — they were causing the black line
            Renderer r = dash.GetComponent<Renderer>();
            r.material         = dashMaterial;
            r.shadowCastingMode    = UnityEngine.Rendering.ShadowCastingMode.Off;
            r.receiveShadows   = false;

            Destroy(dash.GetComponent<BoxCollider>());

            currentZ += step;
            index++;
        }

        Debug.Log($"[RoadDivider] {index} dashes generated.");
    }

    void OnDrawGizmos()
    {
        float y         = ROAD_Y + ROAD_SCALE_Y / 2f + 0.012f;
        float roadStart = ROAD_Z - ROAD_SCALE_Z / 2f;
        float roadEnd   = ROAD_Z + ROAD_SCALE_Z / 2f;
        float step      = dashLength + gapLength;
        float z         = roadStart;

        Gizmos.color = Color.white;
        while (z + dashLength <= roadEnd)
        {
            Gizmos.DrawLine(new Vector3(ROAD_X, y, z), new Vector3(ROAD_X, y, z + dashLength));
            z += step;
        }

        Gizmos.color = new Color(1f, 1f, 0f, 0.3f);
        Gizmos.DrawWireCube(new Vector3(ROAD_X, ROAD_Y, ROAD_Z), new Vector3(10f, ROAD_SCALE_Y, ROAD_SCALE_Z));
    }
}