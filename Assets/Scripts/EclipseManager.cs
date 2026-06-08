using System;
using System.Collections;
using UnityEngine;

public class EclipseManager : MonoBehaviour
{
    public bool isEclipseActive = false;
    public AnimationCurve curve;
    public AnimationCurve screamingCurve;
    public float maxDuration = 7f;
    public float strengthDampening = 0.1f;
    public float screamingDampening = 0.1f;
    public AudioSource screamAudioSource;

    void Start()
    {
        EclipseDetector.OnEclipseStart += HandleEclipseStart;
        EclipseDetector.OnEclipseEnd += HandleEclipseEnd;
    }

    private void HandleEclipseStart()
    {
        isEclipseActive = true;
        StartCoroutine(Shaking());
        StartCoroutine(Screaming());
    }

    private void HandleEclipseEnd()
    {
        isEclipseActive = false;
    }

    IEnumerator Shaking()
    {
        Vector3 originalPosition = transform.position;
        float elapsedTime = 0f;
        float strength = 0f;
        while (isEclipseActive)
        {
            elapsedTime += Time.deltaTime;
            strength = curve.Evaluate(Math.Min(1f, elapsedTime / maxDuration));
            transform.position = originalPosition + UnityEngine.Random.insideUnitSphere * strength;
            yield return null;
        }

        while (strength > 0f)
        {
            strength -= strengthDampening * Time.deltaTime;
            transform.position = originalPosition + UnityEngine.Random.insideUnitSphere * strength;
            yield return null;
        }

        transform.position = originalPosition;
    }

    IEnumerator Screaming()
    {
        float elapsedTime = 0f;
        float screamingStrength = 0f;

        screamAudioSource.volume = 0f;
        screamAudioSource.Play();

        while (isEclipseActive)
        {
            elapsedTime += Time.deltaTime;
            screamingStrength = screamingCurve.Evaluate(Math.Min(1f, elapsedTime / maxDuration));
            screamAudioSource.volume = screamingStrength;
            yield return null;
        }

        while (screamingStrength > 0f)
        {
            screamingStrength -= screamingDampening * Time.deltaTime;
            screamAudioSource.volume = Mathf.Max(0f, screamingStrength);
            yield return null;
        }

        screamAudioSource.Stop();
    }
}