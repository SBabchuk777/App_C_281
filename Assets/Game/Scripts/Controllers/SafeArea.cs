using UnityEngine;

public class SafeArea : MonoBehaviour
{
    void Awake()
    {
        UpdateSafeArea();
    }

    private void UpdateSafeArea()
    {
        var safeArea = Screen.safeArea;
        var myReactTransform = GetComponent<RectTransform>();

        var anchorMin = safeArea.position;
        var anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        myReactTransform.anchorMin = anchorMin;
        myReactTransform.anchorMax = anchorMax;
    }
}
