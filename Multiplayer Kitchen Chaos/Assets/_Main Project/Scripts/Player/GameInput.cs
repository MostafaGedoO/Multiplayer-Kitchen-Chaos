using System;
using UnityEngine.InputSystem;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance;
    public event EventHandler OnInteractAction;

    private PlayerInputActions playerInputsMap;
    

    private void Awake()
    {
        Instance = this;

        playerInputsMap = new PlayerInputActions();
        playerInputsMap.Player.Enable();

        playerInputsMap.Player.Interact.performed += InteractPerformed;
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
