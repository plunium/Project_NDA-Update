using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Camera Tilt")]
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private float camTilt;
    [SerializeField]
    private float camTiltTime;

    [Header("Player Animation")]
    [SerializeField]
    private Animator _playerAnimator;

    public float tilt { get; private set; }
    private float _timer = 2.0f;
    private int _numberOfColliderUnder = 0;
    public bool isPlayerRagdollActive;
    public bool isRagdollActive { get; set; }
    private bool _isRagdollActive = false;

    void Update()
    {
        if (!_isRagdollActive)
        {
            _timer += Time.deltaTime;

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
                tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                tempSpeed += -transform.right * _lateralSpeed;
                tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
            }

            _rb.velocity = Vector3.Lerp(_rb.velocity, tempSpeed, _acceleration * Time.deltaTime);

            //Mouvement de saut
            if (Input.GetKeyDown(KeyCode.Space) && _numberOfColliderUnder > 0 && _timer > 1.0f)
            {
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
                _playerAnimator.SetTrigger("Slide");
                _timer = 0.0f;
            }

            //Gravité
            if (_rb.velocity.y < -1)
                _rb.AddForce(Physics.gravity * Time.deltaTime * 100);

            if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
                tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime);

            cam.transform.localEulerAngles = new Vector3(0, 0, tilt);

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
