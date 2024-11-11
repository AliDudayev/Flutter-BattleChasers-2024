using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushEffect : MonoBehaviour
{
    private List<Color> originalColors = new List<Color>();  // Stores original colors of all materials
    private List<Renderer> childRenderers = new List<Renderer>();  // Stores all renderers in child objects
    private Rigidbody rb;

    [Header("Settings")]
    [SerializeField] private float pushForce = 5f;
    [SerializeField] private float colorChangeDuration = 1f;
    [SerializeField] private float pushStopDuration = 0.5f;  // Time in seconds to reduce push effect

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            childRenderers.Add(renderer);
            foreach (Material material in renderer.materials)
            {
                originalColors.Add(material.color);
            }
        }
    }

    public void ApplyPushback(Vector3 hitDirection)
    {
        if (rb != null)
        {
            // Calculate the push direction and set y to 0 to ensure horizontal-only force
            Vector3 pushDirection = (transform.position - hitDirection).normalized;
            pushDirection.y = 0;

            rb.AddForce(pushDirection * pushForce, ForceMode.Impulse);

            // Start velocity dampening to stop the pushback effect
            StartCoroutine(StopPushbackGradually());
        }

        if (childRenderers.Count > 0)
        {
            StopAllCoroutines();
            StartCoroutine(ChangeColorTemporarily(Color.red, colorChangeDuration));
        }
    }

    private IEnumerator StopPushbackGradually()
    {
        float elapsedTime = 0f;

        // Gradually reduce the velocity over the specified duration
        while (elapsedTime < pushStopDuration)
        {
            if (rb != null)
            {
                rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, elapsedTime / pushStopDuration);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the velocity is completely stopped
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
        }
    }

    private IEnumerator ChangeColorTemporarily(Color hitColor, float duration)
    {
        // Set each material color to hitColor instantly
        for (int i = 0; i < childRenderers.Count; i++)
        {
            foreach (Material material in childRenderers[i].materials)
            {
                material.color = hitColor;
            }
        }

        // Gradually blend back to the original colors
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            for (int i = 0; i < childRenderers.Count; i++)
            {
                int colorIndex = 0;
                foreach (Material material in childRenderers[i].materials)
                {
                    material.color = Color.Lerp(hitColor, originalColors[colorIndex], elapsedTime / duration);
                    colorIndex++;
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure each material color is fully back to its original color
        int finalColorIndex = 0;
        for (int i = 0; i < childRenderers.Count; i++)
        {
            foreach (Material material in childRenderers[i].materials)
            {
                material.color = originalColors[finalColorIndex];
                finalColorIndex++;
            }
        }
    }
}
