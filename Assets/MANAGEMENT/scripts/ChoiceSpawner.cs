using UnityEngine;

public class ChoiceSpawner : MonoBehaviour
{
    [System.Serializable]
    public class ChoicePair
    {
        public GameObject leftChoice;
        public GameObject rightChoice;
    }

    [Header("Choice Pairs")]
    public ChoicePair[] pairs;

    [Header("Road Settings")]
    public float roadCenterX = 400f;
    public float laneOffset = 12f;

    [Header("Spawn Settings")]
    public float startZ = 50f;
    public float distanceBetweenPairs = 50f;
    public float itemY = -0.3f;

    private void Start()
    {
        SpawnPairs();
    }
    

    void SpawnPairs()
    {
        for (int i = 0; i < pairs.Length; i++)
        {
            float zPos = startZ + (i * distanceBetweenPairs);

             Vector3 leftPos = new Vector3(roadCenterX - laneOffset, itemY, zPos);
            Vector3 rightPos = new Vector3(roadCenterX + laneOffset, itemY, zPos);

            Instantiate(pairs[i].leftChoice, leftPos, Quaternion.identity);
            Instantiate(pairs[i].rightChoice, rightPos, Quaternion.identity);
        }
    }
}