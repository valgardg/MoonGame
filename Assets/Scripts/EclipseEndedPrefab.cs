using TMPro;
using UnityEngine;

public class EclipseEndedPrefab : MonoBehaviour
{
    public TMP_Text text;

    public void Initialize(float seconds)
    {
        text.text = $"Eclipse lasted {seconds:F2} seconds!";
    }
}
