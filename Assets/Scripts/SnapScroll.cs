using UnityEngine;
using UnityEngine.UI;

public class SnapScroll : MonoBehaviour
{
    public ScrollRect scrollRect;
    public int itemCount;
    private float[] positions;
    private bool isDragging;

    void Start()
    {
        positions = new float[itemCount];
        float step = 1f / (itemCount - 1);

        for (int i = 0; i < itemCount; i++)
        {
            positions[i] = step * i;
        }
    }

    public void OnBeginDrag()
    {
        isDragging = true;
    }

    public void OnEndDrag()
    {
        isDragging = false;
        SnapToClosest();
    }

    void SnapToClosest()
    {
        float current = scrollRect.horizontalNormalizedPosition;
        float closest = positions[0];
        float distance = Mathf.Abs(current - closest);

        for (int i = 1; i < positions.Length; i++)
        {
            float d = Mathf.Abs(current - positions[i]);
            if (d < distance)
            {
                distance = d;
                closest = positions[i];
            }
        }

        scrollRect.horizontalNormalizedPosition = closest;
    }
}