using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SegmentGenerator : MonoBehaviour
{
    public GameObject[] segment;
    [SerializeField] int zPos = 50;
    [SerializeField] bool creatingSegment = false;
    [SerializeField] int segmentNum;

    void Awake()
    {
        zPos = 50;
        creatingSegment = false;
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name != "Run") return;

        if (creatingSegment == false)
        {
            creatingSegment = true;
            StartCoroutine(SegmentGen());
        }
    }

    IEnumerator SegmentGen()
    {
        segmentNum = Random.Range(0, 3);
        
        // THE FIX: We use 'null' or no parent to ensure it spawns at EXACTLY (0, 0, zPos) 
        // in the world, ignoring the LevelControls' own position.
        Instantiate(segment[segmentNum], new Vector3(0, 0, zPos), Quaternion.identity);
        
        zPos += 50;
        yield return new WaitForSeconds(5);
        creatingSegment = false;
    }
}
