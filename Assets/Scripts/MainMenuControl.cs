using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuControl : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject fadeOut;
    [SerializeField] AudioSource buttonSelect;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //new method for starting button
    public void StartGame()
    {
        StartCoroutine(StartButton());
    }
    IEnumerator StartButton()
    {
        buttonSelect.Play();
        fadeOut.SetActive(true);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(1);
    }
}
