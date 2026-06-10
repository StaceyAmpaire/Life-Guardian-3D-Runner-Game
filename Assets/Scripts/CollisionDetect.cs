using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CollisionDetect : MonoBehaviour
{
    [SerializeField] GameObject thePlayer;
    [SerializeField] GameObject playerAnim;
    [SerializeField] AudioSource collisionFX;
    [SerializeField] GameObject mainCam;
    [SerializeField] GameObject fadeOut;

    void Start()
{
    
    thePlayer = GameObject.Find("Guardian");
    playerAnim = GameObject.Find("Running (1)");
    mainCam = GameObject.Find("Main Camera");
    fadeOut = GameObject.Find("FadeOut");

    collisionFX = GameObject.Find("CollisionFX")
                            .GetComponent<AudioSource>();
}

    void OnTriggerEnter(Collider other)
    {
        StartCoroutine(CollisionEnd());
    }

    IEnumerator CollisionEnd()
    {
        collisionFX.Play();
        thePlayer.GetComponent<PlayerMovement>().enabled = false;
        playerAnim.GetComponent<Animator>().Play("Stumblerunner1");
        mainCam.GetComponent<Animator>().Play("CollisionCam");

        yield return new WaitForSeconds(2);

//Debug.Log("Fade Trigger Fired");
fadeOut.GetComponent<Animator>()
       .SetTrigger("Fade");

yield return new WaitForSeconds(3);

FindFirstObjectByType<EndRunUI>().ShowPopup();
    }
}