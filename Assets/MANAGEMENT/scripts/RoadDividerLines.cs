using UnityEngine;

public class RoadDividerLines : MonoBehaviour
{
    [Header("Road")]
    public float roadCenterX = 400f;
    public float halfRoadWidth = 20f;
    public float roadLength = 3000f;

    [Header("Divider Lines")]
    public Material lineMaterial;
    public float segmentLength = 8f;
    public float gapLength = 3f;

    [Header("Kerbs")]
    public Material redMaterial;
    public Material whiteMaterial;

    void Start()
    {
        // Two dotted lane dividers
        CreateDottedLine(roadCenterX - 5.5f);
        CreateDottedLine(roadCenterX + 5.5f);

        // Left and right kerbs
        CreateKerb(roadCenterX - halfRoadWidth);
        CreateKerb(roadCenterX + halfRoadWidth);
    }

    void CreateDottedLine(float x)
    {
        for (float z = 0; z < roadLength; z += segmentLength + gapLength)
        {
            GameObject segment = GameObject.CreatePrimitive(PrimitiveType.Cube);

            segment.name = "Divider Segment";

            segment.transform.position =
                new Vector3(x, 0.06f, z);

            segment.transform.localScale =
                new Vector3(0.2f, 0.05f, segmentLength);

            if (lineMaterial != null)
                segment.GetComponent<Renderer>().material = lineMaterial;
        }
    }

    void CreateKerb(float x)
    {
        int count = 0;

        for (float z = 0; z < roadLength; z += 2f)
        {
            GameObject kerb = GameObject.CreatePrimitive(PrimitiveType.Cube);

            kerb.name = "Kerb";

            kerb.transform.position =
                new Vector3(x, 0.15f, z);

            kerb.transform.localScale =
                new Vector3(1f, 0.25f, 2f);

            Renderer r = kerb.GetComponent<Renderer>();

            if (count % 2 == 0)
            {
                if (redMaterial != null)
                    r.material = redMaterial;
            }
            else
            {
                if (whiteMaterial != null)
                    r.material = whiteMaterial;
            }

            count++;
        }
    }
}