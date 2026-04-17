using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;

public class PreparationPhaseSwitch : MonoBehaviour
{
    [SerializeField] private InputActionReference startDefenseAction;

    [Header("Cameras")]
    [SerializeField] private CinemachineCamera fpsCamera;
    [SerializeField] private CinemachineCamera topViewCamera;
    [SerializeField] private Camera mainCamera;

    [Header("Player")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private AimController aimController;

    [SerializeField] private bool isPreparationMode;

    private void Start()
    {
        isPreparationMode = false;
        ApplyCursorState();
        ApplyGameState();
    }

    public void OnEnable()
    {        
        startDefenseAction.action.Enable();
        startDefenseAction.action.performed += ShowCursor;
    }



    public void OnDisable()
    {
        startDefenseAction.action.performed -= ShowCursor;
        startDefenseAction.action.Disable();        
    }

    private void ShowCursor(InputAction.CallbackContext context)
    {
        Debug.Log("E key pressed");
        isPreparationMode = !isPreparationMode;
        ApplyCursorState();
        ApplyGameState();
    }

    private void ApplyCursorState()
    {
        Cursor.visible = isPreparationMode;
        Cursor.lockState = isPreparationMode ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void ApplyGameState()
    {
        playerMovement.enabled = !isPreparationMode;
        aimController.enabled = !isPreparationMode;

        ChangeCameraView();
    }

    private void ChangeCameraView()
    {
        if (isPreparationMode)
        {
            // FPS mode
            fpsCamera.Priority = 0;
            topViewCamera.Priority = 10;
            StartCoroutine(ChangeToOrtographic());
                       
        }
        else
        {
            // Preparation mode
            fpsCamera.Priority = 10;
            topViewCamera.Priority = 0;
            mainCamera.orthographic = false;
        }
    }

    private IEnumerator ChangeToOrtographic()
    {
        yield return new WaitForSeconds(1.5f);
        topViewCamera.Lens.OrthographicSize = 20f;
        mainCamera.orthographic = true;
    }
}
