using System;
using System.Collections;
using UnityEngine;

public class PlanetController : MonoBehaviour
{
    // Reference to the planet the moon is orbiting around
    [SerializeField] private Transform targetCelestialBody;
    [SerializeField] private float angleOffset = 0f;

    [Header("Eclipse Fade Settings")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float fadeOutDuration = 2f;
    [SerializeField] private float fadeInDuration = 0.4f;
    [SerializeField] private float minAlpha = 0.1f;
    [SerializeField] private AnimationCurve fadeOutCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    private float maxAlpha = 1f;
    

    [SerializeField] private float fadeOutSpeed = 1f;
    [SerializeField] private float fadeInSpeed = 2.5f;

    private bool isEclipseActive = false;
    private Coroutine fadeCoroutine;

    void Start()
    {
        EclipseDetector.OnEclipseStart += HandleEclipseStart;
        EclipseDetector.OnEclipseEnd += HandleEclipseEnd;
    }

    void OnDestroy()
    {
        EclipseDetector.OnEclipseStart -= HandleEclipseStart;
        EclipseDetector.OnEclipseEnd -= HandleEclipseEnd;
    }

    void Update()
    {
        RotateTowardsTarget();
        // UpdateAlpha();
    }

    private void HandleEclipseStart()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeAlpha(spriteRenderer.color.a, minAlpha, fadeOutDuration));
        isEclipseActive = true;
    }

    private IEnumerator FadeAlpha(float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            Color c = spriteRenderer.color;
            c.a = Mathf.Lerp(from, to, fadeOutCurve.Evaluate(t));
            spriteRenderer.color = c;
            yield return null;
        }
        Color final = spriteRenderer.color;
        final.a = to;
        spriteRenderer.color = final;
    }

    private void HandleEclipseEnd()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeIn());
        isEclipseActive = false;
    }

    private IEnumerator FadeIn()
    {
        float startAlpha = spriteRenderer.color.a;
        float remainingDistance = 1f - startAlpha;

        // Scale duration proportionally — if we're already at 0.9, this will be fast
        float scaledDuration = fadeInDuration * remainingDistance;

        float elapsed = 0f;
        while (elapsed < scaledDuration)
        {
            elapsed += Time.deltaTime;
            Color c = spriteRenderer.color;
            c.a = Mathf.Lerp(startAlpha, 1f, elapsed / scaledDuration);
            spriteRenderer.color = c;
            yield return null;
        }

        Color final = spriteRenderer.color;
        final.a = 1f;
        spriteRenderer.color = final;
    }

    private void RotateTowardsTarget()
    {
        Vector2 direction = targetCelestialBody.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + angleOffset);
    }
}