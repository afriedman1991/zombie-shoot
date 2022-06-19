using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class SwitchVCam : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private int priorityBoostAmount = 10;
    [SerializeField] private Canvas thirdPersonCanvas;
    [SerializeField] private Canvas aimCanvas;

    private InputAction aimAction;
    private CinemachineVirtualCamera virtalCamera;

    // Start is called before the first frame update
    private void Awake()
    {
        aimAction = playerInput.actions["Aim"];
        virtalCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void OnEnable()
    {
        aimAction.performed += _ => StartAim();
        aimAction.canceled += _ => CancelAim();
    }

    private void OnDisable()
    {
        aimAction.performed -= _ => StartAim();
        aimAction.canceled -= _ => CancelAim();
    }

    private void StartAim()
    {
        virtalCamera.Priority += priorityBoostAmount;
        aimCanvas.enabled = true;
        thirdPersonCanvas.enabled = false;
    }

    private void CancelAim()
    {
        virtalCamera.Priority -= priorityBoostAmount;
        aimCanvas.enabled = false;
        thirdPersonCanvas.enabled = true;
    }
}
