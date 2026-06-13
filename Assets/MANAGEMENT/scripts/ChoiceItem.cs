using UnityEngine;
using TMPro;

public class ChoiceItem : MonoBehaviour
{
    [Header("Choice Info")]
    public string choiceName;

    [TextArea]
    public string message;

    [Header("Points")]
    public int points = 250;

    [Header("Name Label")]
    public TMP_Text nameLabel;

    void Start()
    {
        if (nameLabel != null)
        {
            nameLabel.text = choiceName;

            // Position label above the choice
            nameLabel.transform.localPosition = new Vector3(0f, 2f, 0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        RemyController remy = other.GetComponent<RemyController>();

        if (remy == null) return;

        if (points >= 0)
            remy.AddPoints(points);
        else
            remy.RemovePoints(-points);

        remy.ShowChoiceMessage(message);

        Destroy(gameObject);
    }
}