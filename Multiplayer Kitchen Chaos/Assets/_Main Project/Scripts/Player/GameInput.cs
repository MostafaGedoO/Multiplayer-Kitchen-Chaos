using System;
using UnityEngine.InputSystem;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance;
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;

    private PlayerInputActions playerInputsMap;
    

    private void Awake()
    {
        Instance = this;

        playerInputsMap = new PlayerInputActions();
        playerInputsMap.Player.Enable();

        playerInputsMap.Player.Interact.performed += InteractPerformed;
        playerInputsMap.Player.InteractAlternate.performed += InteractAlternatePerformed;
        playerInputsMap.Player.Pause.performed += Pause_performed;
    }

    private void OnDestroy()
    {
        playerInputsMap.Player.Interact.performed -= InteractPerformed;
        playerInputsMap.Player.InteractAlternate.performed -= InteractAlternatePerformed;
        playerInputsMap.Player.Pause.performed -= Pause_performed;

        playerInputsMap.Dispose();
    }

    private void Pause_performed(InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternatePerformed(InputAction.CallbackContext context)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractPerformed(InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this,EventArgs.Empty);
    }

    public Vector2 GetMovmentVectorNormalized()
    {
        Vector2 _moveInput = playerInputsMap.Player.Move.ReadValue<Vector2>();

        _moveInput = _moveInput.normalized;

        return _moveInput;
    }
}
