using System;
using UnityEngine;

public class EclipseDetector : MonoBehaviour
{
    public LayerMask moonLayer;
    public Transform target;

    // events
    public static event Action OnEclipseStart;
    public static event Action OnEclipseEnd;

    private bool isEclipseActive;

    void Update()
    {
        bool hasLineOfSight = HasLineOfSight();
        Debug.DrawLine(transform.position, target.position, hasLineOfSight ? Color.green : Color.red);
    

        if (hasLineOfSight && isEclipseActive)
        {
            Debug.Log("Eclipse ended");
            isEclipseActive = false;
            OnEclipseEnd?.Invoke();
        }
        else if (!hasLineOfSight && !isEclipseActive)
        {
            Debug.Log("Eclipse started");
            isEclipseActive = true;
            OnEclipseStart?.Invoke();
        }
    }

    public bool HasLineOfSight()
    {
        Vector2 direction = target.position - transform.position;
        float distance = direction.magnitude;

        return !Physics2D.Raycast(
            transform.position,
            direction.normalized,
            distance,
            moonLayer
        );
    }

    void OnDrawGizmos()
    {
        if (target == null) return;

        Gizmos.color = isEclipseActive ? Color.red : Color.green;
        Gizmos.DrawLine(transform.position, target.position);
    }
}
