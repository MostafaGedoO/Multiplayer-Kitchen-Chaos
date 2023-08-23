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
            Vector2 _moveInput = GameInput.Instance.GetMovmentVectorNormalized();
            Vector3 _moveDir = new Vector3(_moveInput.x, 0f, _moveInput.y);

            //Moving into walls
            float _playerRadius = 0.7f;
            float _moveDistance = moveSpeed * Time.deltaTime;
            float _playerHeight = 2f;

            bool _canMove = !Physics.CapsuleCast(transform.position, transform.position + new Vector3(0, _playerHeight, 0), _playerRadius, _moveDir,_moveDistance);

            if (!_canMove)
            {
                //try moveing on the x only
                Vector3 _moveDirX = new Vector3(_moveDir.x, 0, 0).normalized;
                _canMove = !Physics.CapsuleCast(transform.position, transform.position + new Vector3(0, _playerHeight, 0), _playerRadius, _moveDirX, _moveDistance);

                if(_canMove)
                {
                    //we can move on the x
                    _moveDir = _moveDirX;
                }
                else
                {
                    //cant move on x lets try z
                    Vector3 _moveDirZ = new Vector3(0, 0, _moveDir.z).normalized;
                    _canMove = !Physics.CapsuleCast(transform.position, transform.position + new Vector3(0, _playerHeight, 0), _playerRadius, _moveDirZ, _moveDistance);

                    if (_canMove)
                    {
                        //we can move on the z
                        _moveDir = _moveDirZ;
                    }
                    else
                    {
                        //we cant move any where
                    }
                }    
            }

            if (_canMove)
            {
               transform.position += _moveDir * _moveDistance;
            }

            transform.forward = Vector3.Slerp(transform.forward, _moveDir, Time.deltaTime * rotateSpeed);

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
