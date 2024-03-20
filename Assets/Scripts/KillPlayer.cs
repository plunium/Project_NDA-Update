using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class KillPlayer : MonoBehaviour
{
    public Transform respawnPoint; // Point de respawn du joueur
    public GameObject deathMenuCanvas;

    private bool isPlayerDead = false; // Bool�en pour suivre l'�tat de mort du joueur
    private bool areObstaclesDisabled = false; // Bool�en pour suivre l'�tat de d�sactivation des obstacles

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle") && !isPlayerDead) // V�rifie si le joueur entre en collision avec une zone de mort et n'est pas d�j� mort
        {
            // D�sactiver les BoxCollider des obstacles si ce n'est pas d�j� fait
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

            // R�initialiser la position du joueur au point de respawn
            transform.position = respawnPoint.position;

            // R�activer les BoxCollider des obstacles apr�s un court d�lai
            StartCoroutine(EnableObstacleCollidersAfterDelay());

            // R�initialiser l'�tat de mort du joueur
            isPlayerDead = false;
        }
    }

    private IEnumerator EnableObstacleCollidersAfterDelay()
    {
        // Attendre un court d�lai avant de r�activer les BoxCollider des obstacles
        yield return new WaitForSeconds(0.1f);

        // R�activer les BoxCollider des obstacles
        EnableObstacleColliders();
    }

    private void DisableObstacleColliders()
    {
        // Trouver tous les GameObjects avec le tag "Obstacle" dans la sc�ne
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        // D�sactiver les BoxCollider des obstacles
        foreach (GameObject obstacle in obstacles)
        {
            BoxCollider collider = obstacle.GetComponent<BoxCollider>();
            if (collider != null)
            {
                collider.enabled = false;
            }
        }

        // Marquer les obstacles comme d�sactiv�s
        areObstaclesDisabled = true;
    }

    private void EnableObstacleColliders()
    {
        // Trouver tous les GameObjects avec le tag "Obstacle" dans la sc�ne
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

        // Marquer les obstacles comme activ�s
        areObstaclesDisabled = false;
    }
}
