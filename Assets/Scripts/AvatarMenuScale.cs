using UnityEngine;

public class AvatarMenuScale : MonoBehaviour
{
    Vector3 baseScale = new Vector3(0.3f, 0.2f, 1f);

    void Start()
    {
        transform.localScale = new Vector3(
            baseScale.x * MasterInfo.bodyWeight,
            baseScale.y,
            baseScale.z * MasterInfo.bodyWeight
        );
    }
}