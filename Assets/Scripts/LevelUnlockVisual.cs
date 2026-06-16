using UnityEngine;
using UnityEngine.UI;

public class LevelUnlockVisual : MonoBehaviour
{
    public Image levelImage;
    public Sprite lockedSprite;
    public Sprite unlockedSprite;

    private void Start()
    {
        if (MasterInfo.level2Unlocked)
        {
            levelImage.sprite = unlockedSprite;
        }
        else
        {
            levelImage.sprite = lockedSprite;
        }
    }
}