using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

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
    public string managementScene = "ManagementScene";

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

            // Ignore clicks on UI
if (EventSystem.current != null)
{
    if (Mouse.current != null &&
        EventSystem.current.IsPointerOverGameObject())
        return;

    if (Touchscreen.current != null &&
        Touchscreen.current.primaryTouch.press.isPressed &&
        EventSystem.current.IsPointerOverGameObject(
            Touchscreen.current.primaryTouch.touchId.ReadValue()))
        return;
}

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
    if (AchievementManager.Instance != null)
        AchievementManager.Instance.MarkPathPlayed("Management");

    AlertManager.Instance.ShowAlert("Entering Management Path.");
    SceneManager.LoadScene(managementScene);
    return;
}

    // PREVENTION PORTAL
    if (type == PortalType.Prevention)
    {
        if (life >= 50)
{
    if (AchievementManager.Instance != null)
        AchievementManager.Instance.MarkPathPlayed("Prevention");

    AlertManager.Instance.ShowAlert(
        "Tree Life is healthy enough. Entering Prevention Path."
    );

    SceneManager.LoadScene(startScene);
}
        else
        {
            AlertManager.Instance.ShowAlert(
                $"Tree Life is too low ({life}).\nRecover in the Management Path until Life reaches 50 or above."
            );
        }
    }
}
}