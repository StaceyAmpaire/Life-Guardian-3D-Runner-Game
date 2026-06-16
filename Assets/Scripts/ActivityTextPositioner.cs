using UnityEngine;

[ExecuteInEditMode]
public class ActivityTextPositioner : MonoBehaviour
{
    [Header("References")]
    public GameObject activityModel;
    public GameObject textObject;

    [Header("Position")]
    public float verticalOffset = 1.5f;

    void Update()
    {
        if (activityModel == null || textObject == null)
            return;

        Renderer[] renderers =
            activityModel.GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
            return;

        Bounds bounds = renderers[0].bounds;

        foreach (Renderer r in renderers)
        {
            bounds.Encapsulate(r.bounds);
        }

        textObject.transform.position =
            new Vector3(
                bounds.center.x,
                bounds.max.y + verticalOffset,
                bounds.center.z
            );
    }
}