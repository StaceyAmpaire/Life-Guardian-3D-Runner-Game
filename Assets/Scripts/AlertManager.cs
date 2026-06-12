using UnityEngine;
using TMPro;
using System.Collections;

public class AlertManager : MonoBehaviour
{
    public static AlertManager Instance;

    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text alertText;

    private Coroutine currentRoutine;

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void ShowAlert(string message, float duration = 3f)
    {
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
        }

        currentRoutine = StartCoroutine(AlertRoutine(message, duration));
    }

    private IEnumerator AlertRoutine(string message, float duration)
    {
        panel.SetActive(true);
        alertText.text = message;

        yield return new WaitForSeconds(duration);

        panel.SetActive(false);
    }
}