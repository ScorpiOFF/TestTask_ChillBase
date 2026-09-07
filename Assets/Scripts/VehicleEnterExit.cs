using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class VehicleEnterExit : MonoBehaviour
{
    [Header("Vehicle")]
    [SerializeField] private Behaviour[] carControlComponents;
    [SerializeField] private Rigidbody carRigidbody;
    [SerializeField] private Transform doorPoint;
    [SerializeField] private Transform seatPoint;
    [SerializeField] private Transform exitPoint;
    [SerializeField] private float interactDistance = 3f;

    [Header("Player")]
    [SerializeField] private GameObject playerRoot;
    [SerializeField] private CharacterController playerCharacterController;
    [SerializeField] private ThirdPersonController playerMovementController;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private StarterAssetsInputs playerStarterAssetsInputs;

    [Header("Cameras")]
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private GameObject carCamera;

    private bool isDriving;
    private Transform playerOriginalParent;

    public bool IsDriving => isDriving;

    public bool CanInteractFromOutside =>
    !isDriving &&
    Vector3.Distance(playerRoot.transform.position, doorPoint.position) <= interactDistance;

    private void Start()
    {
        playerOriginalParent = playerRoot.transform.parent;

        SetCarControls(false);
        carCamera.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current == null || !Keyboard.current.eKey.wasPressedThisFrame)
            return;

        if (isDriving)
        {
            ExitVehicle();
            return;
        }

        float distanceToDoor = Vector3.Distance(
            playerRoot.transform.position,
            doorPoint.position);

        if (distanceToDoor <= interactDistance)
            EnterVehicle();
    }

    private void EnterVehicle()
    {
        isDriving = true;

        SetPlayerControls(false);

        playerCharacterController.enabled = false;
        playerRoot.transform.SetParent(seatPoint);
        playerRoot.transform.localPosition = Vector3.zero;
        playerRoot.transform.localRotation = Quaternion.identity;
        playerRoot.SetActive(false);

        playerCamera.SetActive(false);
        carCamera.SetActive(true);
        SetCarControls(true);
    }

    private void ExitVehicle()
    {
        isDriving = false;

        SetCarControls(false);
        carRigidbody.linearVelocity = Vector3.zero;
        carRigidbody.angularVelocity = Vector3.zero;

        playerRoot.SetActive(true);
        playerRoot.transform.SetParent(playerOriginalParent);
        Vector3 flatForward = Vector3.ProjectOnPlane(exitPoint.forward, Vector3.up);

        if (flatForward.sqrMagnitude < 0.001f)
            flatForward = Vector3.forward;

        playerRoot.transform.SetPositionAndRotation(
            exitPoint.position,
            Quaternion.LookRotation(flatForward.normalized, Vector3.up));

        playerCharacterController.enabled = true;
        SetPlayerControls(true);

        carCamera.SetActive(false);
        playerCamera.SetActive(true);
    }

    private void SetCarControls(bool enabled)
    {
        foreach (Behaviour control in carControlComponents)
        {
            if (control != null)
                control.enabled = enabled;
        }
    }

    private void SetPlayerControls(bool enabled)
    {
        if (playerMovementController != null)
            playerMovementController.enabled = enabled;

        if (playerInput != null)
            playerInput.enabled = enabled;

        if (playerStarterAssetsInputs != null)
            playerStarterAssetsInputs.enabled = enabled;
    }
}