using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    public Animator animator;
    public Rigidbody[] rigidbodies;
    public Collider[] colliders;
    public Rigidbody playerRigidbody;

    private bool isRagdoll = false;

    public void EnableRagdoll()
    {
        if (!isRagdoll)
        {
            animator.enabled = false;
            foreach (Rigidbody rb in rigidbodies)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

                Collider collider = rb.GetComponent<Collider>();
                if (collider != null)
                {
                    collider.enabled = true;

                    // Vérifiez si le collider est un BoxCollider, SphereCollider ou CapsuleCollider
                    BoxCollider boxCollider = collider as BoxCollider;
                    SphereCollider sphereCollider = collider as SphereCollider;
                    CapsuleCollider capsuleCollider = collider as CapsuleCollider;

                    if (boxCollider != null)
                    {
                        boxCollider.isTrigger = false;
                    }
                    else if (sphereCollider != null)
                    {
                        sphereCollider.isTrigger = false;
                    }
                    else if (capsuleCollider != null)
                    {
                        capsuleCollider.isTrigger = false;
                    }
                }
            }
            playerRigidbody.isKinematic = true;
            isRagdoll = true;

            PlayerMovement playerMovement = GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.SetRagdollActive(true);
            }
        }
    }

    public void DisableRagdoll()
    {
        animator.enabled = true;
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = true;
        }
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
        playerRigidbody.isKinematic = false;
        isRagdoll = false;

        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.SetRagdollActive(false);
        }
    }
}
