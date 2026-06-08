using System;
using UnityEngine;

public class MoonController : MonoBehaviour
{
    public float orbitSpeed = 4;
    public float thrustForce = 4;
    public float decelerationRate = 0.2f;

    public float minSpeed = 0f;
    public float maxSpeed = 50f;

    private Rigidbody2D rb;
    private DistanceJoint2D joint;

    private float currentSpeed;

    // Reference to the planet the moon is orbiting around
    [SerializeField] private Transform planet;
    [SerializeField] private float angleOffset = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        joint = GetComponent<DistanceJoint2D>();

        currentSpeed = orbitSpeed;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
            currentSpeed += thrustForce * Time.deltaTime;

        else if (Input.GetKey(KeyCode.S))
            currentSpeed -= thrustForce * Time.deltaTime;
        
        else
        {
            // Gradually return to orbit speed when no input is given
            if (currentSpeed > orbitSpeed)
                currentSpeed -= thrustForce * decelerationRate * Time.deltaTime;
            else if (currentSpeed < orbitSpeed)
                currentSpeed += thrustForce * decelerationRate * Time.deltaTime;
        }

        currentSpeed = Math.Clamp(currentSpeed, minSpeed, maxSpeed);

        // rotate towards planet
        Vector2 direction = planet.position - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle + angleOffset);
    }

    void FixedUpdate()
    {
        Vector2 toSun = ((Vector2)transform.position - (Vector2)joint.connectedBody.transform.position).normalized;
        Vector2 tangent = new Vector2(-toSun.y, toSun.x);

        rb.linearVelocity = tangent * currentSpeed;
    }
}