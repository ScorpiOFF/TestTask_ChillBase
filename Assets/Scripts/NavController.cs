using System.Collections.Generic;
using UnityEngine;

public class NavController : MonoBehaviour
{
    [Header("Route")]
    [SerializeField] private List<NavPoint> routePoints;

    [Header("Player")]
    [SerializeField] private Transform playerTarget;
    [SerializeField] private Transform playerArrow;

    [Header("Car")]
    [SerializeField] private Transform carTarget;
    [SerializeField] private Transform carArrow;
    [SerializeField] private VehicleEnterExit vehicleEnterExit;

    private int currentPointIndex = 0;

    private void Start()
    {
        SetCurrentPoint(currentPointIndex);

        if (playerArrow != null)
            playerArrow.gameObject.SetActive(true);

        if (carArrow != null)
            carArrow.gameObject.SetActive(false);
    }

    private void Update()
    {
        Transform navigationTarget = GetCurrentNavigationTarget();
        Transform directionArrow = GetCurrentArrow();

        if (navigationTarget == null)
            return;

        UpdateDirectionArrow(navigationTarget, directionArrow);

        if (currentPointIndex >= routePoints.Count)
            return;

        NavPoint currentPoint = routePoints[currentPointIndex];

        float distance = Vector3.Distance(
            navigationTarget.position,
            currentPoint.transform.position
        );

        if (distance <= currentPoint.ReachDistance)
        {
            GoToNextPoint();
        }
    }

    private Transform GetCurrentNavigationTarget()
    {
        if (vehicleEnterExit != null && vehicleEnterExit.IsDriving)
            return carTarget;

        return playerTarget;
    }

    private Transform GetCurrentArrow()
    {
        if (vehicleEnterExit != null && vehicleEnterExit.IsDriving)
            return carArrow;

        return playerArrow;
    }

    private void UpdateDirectionArrow(
        Transform navigationTarget,
        Transform directionArrow)
    {
        if (directionArrow == null)
            return;

        if (currentPointIndex >= routePoints.Count)
        {
            directionArrow.gameObject.SetActive(false);
            return;
        }

        directionArrow.gameObject.SetActive(true);

        directionArrow.position = navigationTarget.position;

        Vector3 direction =
            routePoints[currentPointIndex].transform.position
            - navigationTarget.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

        directionArrow.rotation = Quaternion.LookRotation(direction);
    }

    private void GoToNextPoint()
    {
        currentPointIndex++;

        if (currentPointIndex >= routePoints.Count)
        {
            CompleteRoute();
            return;
        }

        SetCurrentPoint(currentPointIndex);
    }

    private void SetCurrentPoint(int index)
    {
        for (int i = 0; i < routePoints.Count; i++)
        {
            routePoints[i].SetActive(i == index);
        }
    }

    private void CompleteRoute()
    {
        if (playerArrow != null)
            playerArrow.gameObject.SetActive(false);

        if (carArrow != null)
            carArrow.gameObject.SetActive(false);

        Debug.Log("Route completed!");
    }
}