using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CollisionDetect : MonoBehaviour
{
    [SerializeField] GameObject thePlayer;
    private Animator maleAnimator;
private Animator femaleAnimator;
    [SerializeField] AudioSource collisionFX;
    [SerializeField] GameObject mainCam;
    [SerializeField] GameObject fadeOut;

    void Start()
{
    
    thePlayer = GameObject.Find("Guardian");
   GameObject male = GameObject.Find("Running (1)");
GameObject female = GameObject.Find("Running (2)");

if (male != null)
    maleAnimator = male.GetComponent<Animator>();

if (female != null)
    femaleAnimator = female.GetComponent<Animator>();
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
        if (maleAnimator != null)
    maleAnimator.Play("Stumblerunner1");

if (femaleAnimator != null)
    femaleAnimator.Play("Stumblerunner1");
        mainCam.GetComponent<Animator>().Play("CollisionCam");

        yield return new WaitForSeconds(2);

//Debug.Log("Fade Trigger Fired");
fadeOut.GetComponent<Animator>()
       .SetTrigger("Fade");

yield return new WaitForSeconds(3);

FindFirstObjectByType<EndRunUI>().ShowPopup();
    }
}