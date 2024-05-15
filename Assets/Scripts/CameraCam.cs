using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCam : MonoBehaviour
{
    public Transform target; // Cible à suivre (le corps du joueur)
    public Vector3 offset; // Décalage entre la caméra et la cible
    public float smoothSpeed = 0.125f; // Vitesse de suivi de la caméra
    public bool isRagdollActive = false;
    void LateUpdate()
    {
        if (target == null || !isRagdollActive)
            return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target);
    }
}
