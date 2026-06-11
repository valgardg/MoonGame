using UnityEngine;

public class TextDisappear : MonoBehaviour
{
    public float disappearTime = 2f; // Time in seconds before the text disappears

    private void Start()
    {
        // Start the coroutine to make the text disappear after the specified time
        StartCoroutine(DisappearAfterTime());
    }

    private System.Collections.IEnumerator DisappearAfterTime()
    {
        // Wait for the specified time
        yield return new WaitForSeconds(disappearTime);

        // Destroy the game object to make the text disappear
        Destroy(gameObject);
    }
}
