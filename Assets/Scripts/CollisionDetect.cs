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
    void OnTriggerEnter(Collider other)
    {
        StartCoroutine(CollisionEnd());
      
}

IEnumerator CollisionEnd()
    {
         collisionFX.Play();
       thePlayer.GetComponent<PlayerMovement>().enabled = false;
       //it will stop moving and give us room to play the animation
       playerAnim.GetComponent<Animator>().Play("Stumble Backwards (1)");
       mainCam.GetComponent<Animator>().Play("CollisionCam");
       yield return new WaitForSeconds(3);
       fadeOut.SetActive(true);
       yield return new WaitForSeconds(3);
         FindFirstObjectByType<EndRunUI>().ShowPopup();
         

    }

}
