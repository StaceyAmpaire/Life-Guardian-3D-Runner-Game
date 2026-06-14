using TMPro;
using UnityEngine;

public class TotalDewDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text dewText;

    void Start()
    {
        dewText.text =
            "Healing Dew: " + MasterInfo.totalDewCount;
    }
}