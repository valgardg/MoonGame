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
    public AudioSource ambientAudioSource;

    public float ambientFadeSpeed = 2f;
    public float ambientMaxVolume = 0.5f;


    private float elapsedTime = 0f;
    private float screamingStrength = 0f;

    void Start()
    {
        screamAudioSource.volume = 0f;
        screamAudioSource.Play();
        ambientAudioSource.volume = ambientMaxVolume;
        ambientAudioSource.Play();
        EclipseDetector.OnEclipseStart += HandleEclipseStart;
        EclipseDetector.OnEclipseEnd += HandleEclipseEnd;
    }

    void Update()
    {
        Screaming();
    }

    private void HandleEclipseStart()
    {
        isEclipseActive = true;
        StartCoroutine(Shaking());
        // StartCoroutine(Screaming());
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

    private void Screaming()
    {
        if (isEclipseActive)
        {
            elapsedTime += Time.deltaTime;
            screamingStrength = screamingCurve.Evaluate(Math.Min(1f, elapsedTime / maxDuration));
            screamAudioSource.volume = screamingStrength;
        }
        else if (screamingStrength > 0f)
        {
            elapsedTime = 0f;
            screamingStrength -= screamingDampening * Time.deltaTime;
            screamAudioSource.volume = Mathf.Max(0f, screamingStrength);
        }
        else
        {
            elapsedTime = 0f;
        }

        ambientAudioSource.volume = Mathf.Clamp01(1f - screamingStrength * ambientFadeSpeed) * ambientMaxVolume;
    }
}