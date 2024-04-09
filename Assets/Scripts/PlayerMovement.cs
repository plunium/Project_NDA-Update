using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField]
    private float _forwardSpeed;
    [SerializeField]
    private float _lateralSpeed;
    [SerializeField]
    private float _jumpForce;
    [SerializeField]
    private float _acceleration = 10;
    [SerializeField]
    private Rigidbody _rb;

    [Header("Camera movement")]
    [SerializeField]
    private CameraFollow _cam;

    [Header("Player Animation")]
    [SerializeField]
    private Animator _playerAnimator;


    private float _timer = 2.0f;
    private int _numberOfColliderUnder = 0;
    public bool isPlayerRagdollActive;
    public bool isRagdollActive { get; set; }
    private bool _isRagdollActive = false;
    private bool _isSliding = false;
    private bool _isJumping = false;

    public bool IsSliding()
    {
        return _isSliding;
    }
    void Update()
    {
        if (!_isRagdollActive)
        {
            _timer += Time.deltaTime;

            if (_isSliding && (_timer > 1.0f))
            {
                _isSliding = false;
                _cam.setRun();
            }
            else if (_isJumping && (_timer > 0.9f))
            {
                _isJumping = false;
                _cam.setRun();
            }

            float forwardDelta = _forwardSpeed * Time.deltaTime;
            float lateralDelta = _lateralSpeed * Time.deltaTime;

            Vector3 currentSpeed = _rb.velocity;

            Vector3 tempSpeed = currentSpeed;

            //mouvement avant
            tempSpeed = transform.forward * _forwardSpeed;

            //On conserve la vitesse verticale
            tempSpeed.y = _rb.velocity.y;

            // Mouvement horizontal
            if (Input.GetKey(KeyCode.RightArrow))
            {
                tempSpeed += transform.right * _lateralSpeed;
                _cam.setTurnRight();
                //_playerAnimator.SetTrigger("Turn_Right");
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                tempSpeed += -transform.right * _lateralSpeed;
                _cam.setTurnLeft();
            }

            _rb.velocity = Vector3.Lerp(_rb.velocity, tempSpeed, _acceleration * Time.deltaTime);

            //Mouvement de saut
            if (Input.GetKeyDown(KeyCode.Space) && _numberOfColliderUnder > 0 && _timer > 1.0f)
            {
                _isJumping = true;
                _rb.AddForce(new Vector3(0, _jumpForce, 0));
                _playerAnimator.SetTrigger("Jump");
                _timer = 0.0f;
            }

            //Mouvement de slide
            // en QWERTY Z = W
            if ((Input.GetKeyDown(KeyCode.Z)
                || Input.GetKeyDown(KeyCode.W)
                || Input.GetKeyDown(KeyCode.LeftShift))
                && _numberOfColliderUnder > 0
                && _timer > 1.0f)
            {
                _isSliding = true;
                _playerAnimator.SetTrigger("Slide");
                _timer = 0.0f;
                _cam.setSlide();
            }

            //Gravité
            if (_rb.velocity.y < -1)
                _rb.AddForce(Physics.gravity * Time.deltaTime * 100);

            if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
            {
                //_playerAnimator.SetTrigger("Run");
                _cam.setRun();
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _numberOfColliderUnder++;
    }


    private void OnTriggerExit(Collider other)
    {
        _numberOfColliderUnder--;
    }

    public void SetRagdollActive(bool isActive)
    {
        _isRagdollActive = isActive;
    }
} 