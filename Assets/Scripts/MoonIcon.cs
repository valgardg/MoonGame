using UnityEngine;

public class MoonIcon : MonoBehaviour
{
    public GameObject moonIcon;

    void Start()
    {
        moonIcon.SetActive(false);
        EclipseDetector.OnEclipseStart += ShowIcon;
        EclipseDetector.OnEclipseEnd += HideIcon;
    }

    public void ShowIcon()
    {
        moonIcon.SetActive(true);
    }

    public void HideIcon()
    {
        moonIcon.SetActive(false);
    }
}