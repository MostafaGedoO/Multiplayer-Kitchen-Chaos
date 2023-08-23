using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotateSpeed = 7f;

    private bool isWalking;

    private void Update()
    {
        if (Input.anyKey)
        {
            Vector2 _moveInput = new Vector2(0, 0);

            if (Input.GetKey(KeyCode.W)) _moveInput.y = +1;
            if (Input.GetKey(KeyCode.S)) _moveInput.y = -1;
            if (Input.GetKey(KeyCode.D)) _moveInput.x = +1;
            if (Input.GetKey(KeyCode.A)) _moveInput.x = -1;

            _moveInput = _moveInput.normalized;

            Vector3 _moveDir = new Vector3(_moveInput.x, 0f, _moveInput.y);
            transform.forward = Vector3.Slerp(transform.forward, _moveDir, Time.deltaTime * rotateSpeed);
            transform.position += _moveDir * moveSpeed * Time.deltaTime;

            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    public bool IsWalking()
    {
        return isWalking;
    }
}
