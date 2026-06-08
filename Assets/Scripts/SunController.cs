using UnityEngine;

public class SunController : MonoBehaviour
{
    public float orbitSpeed = 4;

    private Rigidbody2D rb;
    private DistanceJoint2D joint;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        joint = GetComponent<DistanceJoint2D>();
    }

    void FixedUpdate()
    {
        Vector2 toSun = ((Vector2)transform.position - (Vector2)joint.connectedBody.transform.position).normalized;
        Vector2 tangent = new Vector2(-toSun.y, toSun.x);

        rb.linearVelocity = tangent * orbitSpeed;

    }

    [SerializeField] private Transform planet;
    [SerializeField] private float angleOffset = 0f;

    void Update()
    {
        Vector2 direction = planet.position - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle + angleOffset);
    }
}
