using UnityEngine;

namespace BaobabGuardian.LevelSelect
{
    /// <summary>
    /// Controls the Baobab tree shader animations.
    /// Manages glow pulsing, leaf swaying, and energy flow effects.
    /// The sprite itself never moves - all animation happens in the shader.
    /// </summary>
    public class BaobabTreeAnimator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        [Header("Glow Settings")]
        [SerializeField] private Color glowColor = new Color(1f, 0.84f, 0f, 1f);
        [SerializeField] private float glowIntensity = 1f;
        [SerializeField] private float glowPulseSpeed = 2f;
        
        [Header("Leaf Sway Settings")]
        [SerializeField] private float leafSwayAmount = 0.1f;
        [SerializeField] private float leafSwaySpeed = 2f;
        
        [Header("Energy Flow Settings")]
        [SerializeField] private float energyFlow = 0.5f;
        [SerializeField] private float energySpeed = 1f;
        
        [Header("Tree Health State")]
        [SerializeField] private TreeHealthState currentHealth = TreeHealthState.Healthy;
        
        private Material treeMaterial;
        private float baseGlowIntensity;

        public enum TreeHealthState
        {
            Healthy,
            Stable,
            Recovering,
            Withering
        }

        private void Start()
        {
            // Get the material from the sprite renderer
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                Debug.LogError("BaobabTreeAnimator: SpriteRenderer not found on this GameObject!");
                return;
            }

            // Create a unique material instance for this tree
            treeMaterial = new Material(spriteRenderer.material);
            spriteRenderer.material = treeMaterial;

            baseGlowIntensity = glowIntensity;
            ApplyHealthState(currentHealth);
        }

        private void Update()
        {
            if (treeMaterial == null) return;

            // Update shader parameters
            UpdateShaderParameters();
        }

        /// <summary>
        /// Updates all shader parameters based on current settings
        /// </summary>
        private void UpdateShaderParameters()
        {
            treeMaterial.SetColor("_GlowColor", glowColor);
            treeMaterial.SetFloat("_GlowIntensity", glowIntensity);
            treeMaterial.SetFloat("_GlowPulseSpeed", glowPulseSpeed);
            treeMaterial.SetFloat("_LeafSwayAmount", leafSwayAmount);
            treeMaterial.SetFloat("_LeafSwaySpeed", leafSwaySpeed);
            treeMaterial.SetFloat("_EnergyFlow", energyFlow);
            treeMaterial.SetFloat("_EnergySpeed", energySpeed);
        }

        /// <summary>
        /// Changes the tree's health state and adjusts visual effects accordingly
        /// </summary>
        public void SetHealthState(TreeHealthState newHealth)
        {
            currentHealth = newHealth;
            ApplyHealthState(newHealth);
        }

        /// <summary>
        /// Applies visual changes based on health state
        /// </summary>
        private void ApplyHealthState(TreeHealthState health)
        {
            switch (health)
            {
                case TreeHealthState.Healthy:
                    // Vibrant green glow, strong energy flow
                    glowColor = new Color(1f, 0.84f, 0f, 1f); // Golden
                    glowIntensity = 1.2f;
                    glowPulseSpeed = 2f;
                    leafSwayAmount = 0.1f;
                    leafSwaySpeed = 2f;
                    energyFlow = 0.7f;
                    energySpeed = 1.5f;
                    break;

                case TreeHealthState.Stable:
                    // Steady glow, moderate energy
                    glowColor = new Color(0.8f, 0.9f, 0.2f, 1f); // Yellow-green
                    glowIntensity = 0.8f;
                    glowPulseSpeed = 1.5f;
                    leafSwayAmount = 0.08f;
                    leafSwaySpeed = 1.5f;
                    energyFlow = 0.5f;
                    energySpeed = 1f;
                    break;

                case TreeHealthState.Recovering:
                    // Dim glow, slow energy flow
                    glowColor = new Color(0.6f, 0.7f, 0.3f, 1f); // Muted green
                    glowIntensity = 0.5f;
                    glowPulseSpeed = 1f;
                    leafSwayAmount = 0.05f;
                    leafSwaySpeed = 1f;
                    energyFlow = 0.3f;
                    energySpeed = 0.5f;
                    break;

                case TreeHealthState.Withering:
                    // Very dim, barely visible energy
                    glowColor = new Color(0.4f, 0.5f, 0.2f, 1f); // Dark green
                    glowIntensity = 0.2f;
                    glowPulseSpeed = 0.5f;
                    leafSwayAmount = 0.02f;
                    leafSwaySpeed = 0.5f;
                    energyFlow = 0.1f;
                    energySpeed = 0.2f;
                    break;
            }

            UpdateShaderParameters();
        }

        /// <summary>
        /// Animates the tree to a new health state over time
        /// </summary>
        public void TransitionToHealthState(TreeHealthState newHealth, float duration = 2f)
        {
            StartCoroutine(TransitionCoroutine(newHealth, duration));
        }

        private System.Collections.IEnumerator TransitionCoroutine(TreeHealthState newHealth, float duration)
        {
            float elapsed = 0f;
            
            // Store starting values
            Color startGlowColor = glowColor;
            float startGlowIntensity = glowIntensity;
            float startGlowPulseSpeed = glowPulseSpeed;
            float startLeafSwayAmount = leafSwayAmount;
            float startLeafSwaySpeed = leafSwaySpeed;
            float startEnergyFlow = energyFlow;
            float startEnergySpeed = energySpeed;

            // Set target values
            ApplyHealthState(newHealth);
            Color targetGlowColor = glowColor;
            float targetGlowIntensity = glowIntensity;
            float targetGlowPulseSpeed = glowPulseSpeed;
            float targetLeafSwayAmount = leafSwayAmount;
            float targetLeafSwaySpeed = leafSwaySpeed;
            float targetEnergyFlow = energyFlow;
            float targetEnergySpeed = energySpeed;

            // Interpolate over time
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                glowColor = Color.Lerp(startGlowColor, targetGlowColor, t);
                glowIntensity = Mathf.Lerp(startGlowIntensity, targetGlowIntensity, t);
                glowPulseSpeed = Mathf.Lerp(startGlowPulseSpeed, targetGlowPulseSpeed, t);
                leafSwayAmount = Mathf.Lerp(startLeafSwayAmount, targetLeafSwayAmount, t);
                leafSwaySpeed = Mathf.Lerp(startLeafSwaySpeed, targetLeafSwaySpeed, t);
                energyFlow = Mathf.Lerp(startEnergyFlow, targetEnergyFlow, t);
                energySpeed = Mathf.Lerp(startEnergySpeed, targetEnergySpeed, t);

                UpdateShaderParameters();
                yield return null;
            }

            // Ensure final values are exact
            ApplyHealthState(newHealth);
        }

        /// <summary>
        /// Temporarily intensifies the glow (for feedback effects)
        /// </summary>
        public void PulseGlow(float intensity = 2f, float duration = 0.5f)
        {
            StartCoroutine(PulseGlowCoroutine(intensity, duration));
        }

        private System.Collections.IEnumerator PulseGlowCoroutine(float intensity, float duration)
        {
            float originalIntensity = glowIntensity;
            float elapsed = 0f;

            // Ramp up
            while (elapsed < duration * 0.5f)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / (duration * 0.5f);
                glowIntensity = Mathf.Lerp(originalIntensity, intensity, t);
                UpdateShaderParameters();
                yield return null;
            }

            // Ramp down
            elapsed = 0f;
            while (elapsed < duration * 0.5f)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / (duration * 0.5f);
                glowIntensity = Mathf.Lerp(intensity, originalIntensity, t);
                UpdateShaderParameters();
                yield return null;
            }

            glowIntensity = originalIntensity;
            UpdateShaderParameters();
        }

        /// <summary>
        /// Temporarily increases leaf sway (for wind effects)
        /// </summary>
        public void SwayLeaves(float swayAmount = 0.3f, float duration = 1f)
        {
            StartCoroutine(SwayLeavesCoroutine(swayAmount, duration));
        }

        private System.Collections.IEnumerator SwayLeavesCoroutine(float swayAmount, float duration)
        {
            float originalAmount = leafSwayAmount;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                leafSwayAmount = Mathf.Lerp(originalAmount, swayAmount, Mathf.Sin(t * Mathf.PI));
                UpdateShaderParameters();
                yield return null;
            }

            leafSwayAmount = originalAmount;
            UpdateShaderParameters();
        }

        /// <summary>
        /// Gets the current health state
        /// </summary>
        public TreeHealthState GetCurrentHealthState()
        {
            return currentHealth;
        }

        /// <summary>
        /// Resets all animations to default values
        /// </summary>
        public void ResetAnimations()
        {
            SetHealthState(TreeHealthState.Healthy);
        }

        private void OnDestroy()
        {
            // Clean up the material instance
            if (treeMaterial != null)
            {
                Destroy(treeMaterial);
            }
        }
    }
}
