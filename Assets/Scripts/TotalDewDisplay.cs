using TMPro;
using UnityEngine;

public class TotalDewDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text dewText;

    void Update()
    {
        dewText.text =
            "Healing Dew: " + MasterInfo.totalDewCount;
    }
}