using UnityEngine;

public class AvatarMenuScale : MonoBehaviour
{
    Vector3 baseScale = new Vector3(0.4f, 0.4f, 1f);

    void Start()
    {
        transform.localScale = new Vector3(
            baseScale.x * MasterInfo.bodyWeight,
            baseScale.y,
            baseScale.z * MasterInfo.bodyWeight
        );
    }
}