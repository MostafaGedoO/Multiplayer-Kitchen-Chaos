using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance;
    private PlayerInputActions playerInputsMap;

    private void Awake()
    {
        Instance = this;

        playerInputsMap = new PlayerInputActions();
        playerInputsMap.Player.Enable();
    }

    public Vector2 GetMovmentVectorNormalized()
    {
        Vector2 _moveInput = playerInputsMap.Player.Move.ReadValue<Vector2>();

        _moveInput = _moveInput.normalized;

        return _moveInput;
    }
}
