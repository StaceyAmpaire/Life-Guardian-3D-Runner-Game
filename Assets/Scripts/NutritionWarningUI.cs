using System.Collections;
using TMPro;
using UnityEngine;

public class NutritionWarningUI : MonoBehaviour
{
    public static NutritionWarningUI Instance;

    [SerializeField] private TMP_Text warningText;
    [SerializeField] private CanvasGroup canvasGroup;

    private Coroutine currentRoutine;

    // Prevent warning spam
    private bool warningActive = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Hidden when game starts
        canvasGroup.alpha = 0f;
    }

    public void ShowWarning(string message)
    {
        // Ignore new warnings while one is already showing
        if (warningActive)
            return;

        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
        }

        currentRoutine = StartCoroutine(ShowRoutine(message));
    }

    private IEnumerator ShowRoutine(string message)
    {
        warningActive = true;

        warningText.text =
    " Health Tip: " + message;

        float t = 0f;

        // ===== FADE IN =====
        while (t < 1f)
        {
            t += Time.deltaTime * 3f;

            canvasGroup.alpha =
                Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        // Stay visible
        yield return new WaitForSeconds(3f);

        t = 0f;

        // ===== FADE OUT =====
        while (t < 1f)
        {
            t += Time.deltaTime * 3f;

            canvasGroup.alpha =
                Mathf.Lerp(1f, 0f, t);

            yield return null;
        }

        canvasGroup.alpha = 0f;

        warningActive = false;
    }
}