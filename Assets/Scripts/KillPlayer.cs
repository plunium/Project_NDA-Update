using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class KillPlayer : MonoBehaviour
{
    public Transform respawnPoint; // Point de respawn du joueur
    public GameObject deathMenuCanvas;

    private bool isPlayerDead = false; // Booléen pour suivre l'état de mort du joueur
    private bool areObstaclesDisabled = false; // Booléen pour suivre l'état de désactivation des obstacles

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle") && !isPlayerDead) // Vérifie si le joueur entre en collision avec une zone de mort et n'est pas déjà mort
        {
            // Désactiver les BoxCollider des obstacles si ce n'est pas déjà fait
            if (!areObstaclesDisabled)
            {
                DisableObstacleColliders();
            }

            // Marquer le joueur comme mort
            isPlayerDead = true;

            // Afficher le menu de mort
            if (deathMenuCanvas != null)
            {
                deathMenuCanvas.SetActive(true);
            }

            // Réinitialiser la position du joueur au point de respawn
            transform.position = respawnPoint.position;

            // Réactiver les BoxCollider des obstacles après un court délai
            StartCoroutine(EnableObstacleCollidersAfterDelay());

            // Réinitialiser l'état de mort du joueur
            isPlayerDead = false;
        }
    }

    private IEnumerator EnableObstacleCollidersAfterDelay()
    {
        // Attendre un court délai avant de réactiver les BoxCollider des obstacles
        yield return new WaitForSeconds(0.1f);

        // Réactiver les BoxCollider des obstacles
        EnableObstacleColliders();
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

    private void EnableObstacleColliders()
    {
        // Trouver tous les GameObjects avec le tag "Obstacle" dans la scène
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        // Activer les BoxCollider des obstacles
        foreach (GameObject obstacle in obstacles)
        {
            BoxCollider collider = obstacle.GetComponent<BoxCollider>();
            if (collider != null)
            {
                collider.enabled = true;
            }
        }

        // Marquer les obstacles comme activés
        areObstaclesDisabled = false;
    }
}
