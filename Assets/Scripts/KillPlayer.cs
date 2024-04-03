using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class KillPlayer : MonoBehaviour
{
    public Transform respawnPoint; // Point de respawn du joueur
    public GameObject deathMenuCanvas;
    public float impactForce = 50f; // Force d'impulsion appliquée au joueur lorsqu'il entre en collision avec un obstacle

    private bool isPlayerDead = false; // Booléen pour suivre l'état de mort du joueur
    private bool areObstaclesDisabled = false; // Booléen pour suivre l'état de désactivation des obstacles

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && !isPlayerDead) // Vérifie si le joueur entre en collision avec une zone de mort et n'est pas déjà mort
        {
            // Appliquer une force d'impulsion au joueur
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 impactDirection = (transform.position - collision.contacts[0].point).normalized;
                rb.AddForce(impactDirection * impactForce, ForceMode.Impulse);
            }

            // Désactiver les BoxCollider des obstacles si ce n'est pas déjà fait
            if (!areObstaclesDisabled)
            {
                DisableObstacleColliders();
            }

            // Marquer le joueur comme mort
            isPlayerDead = true;

            // Activer le mode ragdoll
            RagdollController ragdollController = GetComponent<RagdollController>();
            PlayerMovement playerMovement = null;
            if (ragdollController != null)
            {
                playerMovement = GetComponent<PlayerMovement>();
                if (isGrounded())
                {
                    ragdollController.EnableRagdoll();
                    transform.position = ragdollController.transform.position;
                }
            }

            // Marquer le mode ragdoll comme actif dans le script PlayerMovement
            if (playerMovement != null)
            {
                playerMovement.isPlayerRagdollActive = true;
            }

            // Afficher le menu de mort
            if (deathMenuCanvas != null)
            {
                deathMenuCanvas.SetActive(true);
            }

            // Attendre que l'animation ragdoll soit terminée avant de réinitialiser la position du joueur
            StartCoroutine(ResetPlayerPositionAfterDelay());
        }
    }

    private IEnumerator ResetPlayerPositionAfterDelay()
    {
        yield return new WaitForSeconds(10f); // Attendre x secondes avant de réinitialiser la position du joueur

        // Désactiver le mode ragdoll
        RagdollController ragdollController = GetComponent<RagdollController>();
        if (ragdollController != null)
        {
            if (GetComponent<PlayerMovement>().isPlayerRagdollActive)
            {
                ragdollController.DisableRagdoll();
                GetComponent<PlayerMovement>().SetRagdollActive(false);
            }
        }

        // Réinitialiser la scène
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private bool isGrounded()
    {
        // Vérifier si le joueur est au sol en utilisant une sphère de rayon 0,1 unité
        // centrée sur la position du joueur et dirigée vers le bas
        RaycastHit hit;
        Vector3 position = transform.position;
        position.y += 0.1f;
        bool isGrounded = Physics.SphereCast(position, 0.1f, Vector3.down, out hit, 0.2f);
        return isGrounded;
    }

    private void DisableObstacleColliders()
    {
        // Trouver tous les GameObjects avec le tag "Obstacle" dans la scène
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        // Désactiver les BoxCollider des obstacles
        foreach (GameObject obstacle in obstacles)
        {
            BoxCollider collider = obstacle.GetComponent<BoxCollider>();
            if (collider != null)
            {
                collider.enabled = false;
            }
        }

        // Marquer les obstacles comme désactivés
        areObstaclesDisabled = true;
    }
}
