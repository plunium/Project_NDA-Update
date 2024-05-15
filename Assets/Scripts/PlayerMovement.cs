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

    [Header("Camera movement")]
    [SerializeField]
    private CameraFollow _cam;

    [Header("Player Animation")]
    [SerializeField]
    private Animator _playerAnimator;
    [SerializeField]
    private string[] _parkourAnimations; // Ajoutez les noms de vos animations de parkour ici dans l'inspecteur Unity

    private float _timer = 2.0f;
    private int _numberOfColliderUnder = 0;
    public bool isPlayerRagdollActive;
    public bool isRagdollActive { get; set; }
    private bool _isRagdollActive = false;
    private bool _isSliding = false;
    private bool _isJumping = false;
    private bool _isParkouring = false; // Ajouté pour le parkour
    private bool _canJump = true;
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
            else if (_isParkouring && (_timer > 2.367f)) // ajuster le temps en fonction de la durée de votre animation de parkour
            {
                _isParkouring = false;
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

            // Détection d'obstacles pour le parkour
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 4f))
            {
                // Si un obstacle est détecté et que la barre d'espace est enfoncée
                if (hit.collider.CompareTag("Obstacle") && Input.GetKeyDown(KeyCode.Space))
                {
                    _isParkouring = true;
                    _rb.AddForce(new Vector3(0, _jumpForce, 0));
                    PlayRandomParkourAnimation();
                    _timer = 0.0f;
                    _canJump = false;
                    StartCoroutine(EnableJumpAfterDelay(1.0f)); // Active le saut après un délai de 1 seconde
                }
            }
            //Mouvement de saut
            else if (_canJump && Input.GetKeyDown(KeyCode.Space) && _numberOfColliderUnder > 0 && _timer > 1.0f)
            {
                _isJumping = true;
                _rb.AddForce(new Vector3(0, _jumpForce, 0));
                _playerAnimator.SetTrigger("Jump");
                _timer = 0.0f;
            }

            // ...

            IEnumerator EnableJumpAfterDelay(float delay)
            {
                yield return new WaitForSeconds(delay);
                _canJump = true;
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
        if (isActive)
        {
            // Vérifie si le Rigidbody est kinematic
            bool wasKinematic = _rb.isKinematic;

            // Si le Rigidbody était kinematic, le rendre non kinematic
            if (wasKinematic)
            {
                _rb.isKinematic = false;
            }

            // Stoppe le Rigidbody lorsque le ragdoll est activé
            _rb.velocity = Vector3.zero;

            // Si le Rigidbody était kinematic, le rendre à nouveau kinematic
            if (wasKinematic)
            {
                _rb.isKinematic = true;
            }
        }
    }

    void PlayRandomParkourAnimation()
    {
        int index = Random.Range(0, _parkourAnimations.Length); // Sélectionne un index aléatoire
        string animationName = _parkourAnimations[index]; // Obtient le nom de l'animation à cet index
        _playerAnimator.Play(animationName, -1, 0f); // Joue l'animation
    }
}

