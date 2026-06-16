using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ClickScript : MonoBehaviour
{
    public enum PortalType
    {
        Prevention,
        Management
    }

    [Header("Portal Settings")]
    public PortalType type;

    [Header("Scene Names")]
    public string startScene = "EnvironmentChoice";
    public string managementScene = "Management";

    void Update()
    {
        Vector2 inputPos = Vector2.zero;
        bool detected = false;

        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            inputPos = Mouse.current.position.ReadValue();
            detected = true;
        }
        else if (Touchscreen.current != null &&
                 Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            inputPos =
                Touchscreen.current.primaryTouch.position.ReadValue();

            detected = true;
        }

        if (!detected)
            return;

        Ray ray = Camera.main.ScreenPointToRay(inputPos);

        RaycastHit2D hit =
            Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null &&
            hit.collider.gameObject == gameObject)
        {
            HandlePortalClick();
        }
    }

    private void HandlePortalClick()
{
    int life = MasterInfo.treeLife;

    // MANAGEMENT PORTAL
    if (type == PortalType.Management)
    {
        if (life < 50)
        {
            AlertManager.Instance.ShowAlert(
                "Tree Life is below 50. Entering Management Path."
            );

            SceneManager.LoadScene(managementScene);
        }
        else
        {
            AlertManager.Instance.ShowAlert(
                $"Management Path is available when Tree Life is below 50.\nCurrent Life: {life}"
            );
        }

        return;
    }

    // PREVENTION PORTAL
    if (type == PortalType.Prevention)
    {
        if (life >= 50)
        {
            AlertManager.Instance.ShowAlert(
                "Tree Life is healthy enough. Entering Prevention Path."
            );

            SceneManager.LoadScene(startScene);
        }
        else
        {
            AlertManager.Instance.ShowAlert(
                $"Prevention Path unlocks at Tree Life 50 or higher.\nCurrent Life: {life}"
            );
        }
    }
}
}