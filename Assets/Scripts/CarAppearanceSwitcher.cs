using UnityEngine;
using UnityEngine.InputSystem;

public class CarAppearanceSwitcher : MonoBehaviour
{
    [SerializeField] private VehicleEnterExit vehicleEnterExit;
    [SerializeField] private GameObject[] visualVariants;
    [SerializeField] private int startVariantIndex;

    private int currentVariantIndex;

    private void Awake()
    {
        if (visualVariants == null || visualVariants.Length == 0)
        {
            Debug.LogWarning("CarAppearanceSwitcher: список вариантов пуст.", this);
            enabled = false;
            return;
        }

        currentVariantIndex = Mathf.Clamp(
            startVariantIndex,
            0,
            visualVariants.Length - 1);

        ApplyVariant();
    }

    private void Update()
    {
        if (vehicleEnterExit == null ||
    !vehicleEnterExit.CanInteractFromOutside ||
    Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            currentVariantIndex =
                (currentVariantIndex + 1) % visualVariants.Length;

            ApplyVariant();
        }
    }

    private void ApplyVariant()
    {
        for (int i = 0; i < visualVariants.Length; i++)
        {
            if (visualVariants[i] != null)
                visualVariants[i].SetActive(i == currentVariantIndex);
        }
    }
}