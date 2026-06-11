using UnityEngine;

public class EclipseEventSpawner : MonoBehaviour
{
    [SerializeField] private GameObject eclipseStartPrefab;
    [SerializeField] private GameObject eclipseEndPrefab;
    [SerializeField] private Transform container;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip eclipseEndSound;

    private float eclipseStartTime;

    void OnEnable()
    {
        EclipseDetector.OnEclipseStart += HandleEclipseStart;
        EclipseDetector.OnEclipseEnd += HandleEclipseEnd;
    }

    void OnDisable()
    {
        EclipseDetector.OnEclipseStart -= HandleEclipseStart;
        EclipseDetector.OnEclipseEnd -= HandleEclipseEnd;
    }

    private void HandleEclipseStart()
    {
        eclipseStartTime = Time.time;
        Instantiate(eclipseStartPrefab, container);
    }

    private void HandleEclipseEnd()
    {
        float duration = Time.time - eclipseStartTime;
        GameObject obj = Instantiate(eclipseEndPrefab, container);
        obj.GetComponent<EclipseEndedPrefab>().Initialize(duration);
        audioSource.PlayOneShot(eclipseEndSound);
    }
}