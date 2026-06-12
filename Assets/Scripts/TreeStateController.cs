using UnityEngine;

public class TreeStateController : MonoBehaviour
{
    private int lastLifeValue = -1; // Track life changes
    [Header("Trunk")]
    public SpriteRenderer trunk;

    [Header("Leaves")]
    public SpriteRenderer[] leaves;

    [Header("Glow Scripts")]
    public BaobabGlowController leafGlow;
    public TrunkEnergyGlow trunkGlow;

    [Header("Trunk Sprites (ALL STATES)")]
    public Sprite healthyTrunk;
    public Sprite slightlyReducedTrunk;
    public Sprite stableTrunk;
    public Sprite recoveringTrunk;
    public Sprite semiRecoveredTrunk;
    public Sprite recoveryStartsTrunk;
    public Sprite witheringTrunk;
    public Sprite almostGoneTrunk;
    public Sprite goneTrunk;
    public Sprite perishedTrunk;

    [Header("Leaf Sprites (ALL STATES)")]
    public Sprite healthyLeaves;
    public Sprite slightlyReducedLeaves;
    public Sprite stableLeaves;
    public Sprite recoveringLeaves;
    public Sprite semiRecoveredLeaves;
    public Sprite recoveryStartsLeaves;
    public Sprite witheringLeaves;
    public Sprite almostGoneLeaves;
    public Sprite goneLeaves;
    public Sprite perishedLeaves;

    void Update()
{
    // ONLY run this when life actually changes
    if (MasterInfo.treeLife != lastLifeValue)
    {
        ApplyState(MasterInfo.treeLife);
        lastLifeValue = MasterInfo.treeLife;
    }
}

    void ApplyState(int life)
{
    Sprite trunkSprite = healthyTrunk;
    Sprite leafSprite = healthyLeaves;
    Color32 glow = new Color32(255, 255, 255, 255);
    float sway = 1f;

    // STATE MAPPING (0–255 COLORS)

    if (life >= 90)
    {
        trunkSprite = healthyTrunk;
        leafSprite = healthyLeaves;

        glow = new Color32(255, 230, 77, 255);
        sway = 1.2f;
    }
    else if (life >= 80)
    {
        trunkSprite = slightlyReducedTrunk;
        leafSprite = slightlyReducedLeaves;

        glow = new Color32(242, 217, 77, 245);
        sway = 1.1f;
    }
    else if (life >= 70)
    {
        trunkSprite = stableTrunk;
        leafSprite = stableLeaves;

        glow = new Color32(217, 217, 102, 235);
        sway = 1f;
    }
    else if (life >= 60)
    {
        trunkSprite = recoveryStartsTrunk;
        leafSprite = recoveryStartsLeaves;

        glow = new Color32(204, 204, 89, 225);
        sway = 0.95f;
    }
    else if (life >= 50)
    {
        trunkSprite = recoveringTrunk;
        leafSprite = recoveringLeaves;

        glow = new Color32(179, 191, 77, 215);
        sway = 0.9f;
    }
    else if (life >= 40)
    {
        trunkSprite = semiRecoveredTrunk;
        leafSprite = semiRecoveredLeaves;

       // glow = new Color32(153, 179, 64, 205);
        sway = 0.8f;
    }
    else if (life >= 30)
    {
        trunkSprite = witheringTrunk;
        leafSprite = witheringLeaves;

        glow = new Color32(128, 153, 51, 190);
        sway = 0.7f;
    }
    else if (life >= 20)
    {
        trunkSprite = almostGoneTrunk;
        leafSprite = almostGoneLeaves;

        glow = new Color32(102, 128, 38, 170);
        sway = 0.6f;
    }
    else if (life >= 10)
    {
        trunkSprite = goneTrunk;
        leafSprite = goneLeaves;

        glow = new Color32(76, 89, 25, 140);
        sway = 0.5f;
    }
    else
    {
        trunkSprite = perishedTrunk;
        leafSprite = perishedLeaves;

        glow = new Color32(40, 40, 40, 120);
        sway = 0.3f;
    }

    // APPLY VISUALS
trunk.sprite = trunkSprite;

foreach (var l in leaves)
{
    if (l != null)
    {
        l.sprite = leafSprite;
        l.color = Color.white; // Ensure they aren't stuck on Red or Transparent
        Debug.Log($"Updated GameObject {l.gameObject.name} to sprite {l.sprite.name}");
    }
    else
    {
        Debug.LogError("A slot in the 'leaves' array is EMPTY!");
    }
}

    // TRUNK GLOW
    if (trunkGlow != null)
    {
        // Tell the glow script what the SHAPE should be
        trunkGlow.glowLayer.sprite = trunkSprite; 
        
        // Pass the color, but TrunkEnergyGlow will now handle the Alpha
        trunkGlow.SetBaseColor(glow);
    }

    // SWAY CONTROL
    var swayRoot = GetComponentInChildren<TreeSwayRoot>();
    if (swayRoot != null)
    {
        swayRoot.swayAmountZ = 1.5f * sway;
        swayRoot.swayAmountX = 0.6f * sway;
    }
    Debug.Log("Leaf sprite applied: " + leafSprite.name);
}
} 