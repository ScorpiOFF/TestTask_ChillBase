using UnityEngine;

public class NavPoint : MonoBehaviour
{
    [SerializeField] private float reachDistance = 5f;
    [SerializeField] private GameObject marker;

    public float ReachDistance => reachDistance;
    public void SetActive(bool active)
    {
        if (marker != null)
            marker.SetActive(active);
    }
}