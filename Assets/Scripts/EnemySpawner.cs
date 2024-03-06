using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform path;
    public float spawnInterval = 2f;
    public float spawnDistance = 20f;
    public float spawnVariation = 2f;
    public float lateralSpawnVariation = 2f;
    public int enemiesToSpawn = 3;
    public float maxDistanceToPlayer = 20f; // Ajustez cette valeur en fonction de la distance souhait�e.
    private float nextSpawnTime = 0f;

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            // Spawn seulement lorsque le joueur entre dans la zone.
            if (PlayerEnteredSpawnZone())
            {
                SpawnEnemies();
                nextSpawnTime = Time.time + spawnInterval;
            }
        }

        // V�rifiez la distance entre le joueur et les ennemis et d�truisez-les si n�cessaire.
        DestroyFarEnemies();
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            // Ajoutez une variation al�atoire � la distance de spawn.
            float randomSpawnDistance = Random.Range(spawnDistance - spawnVariation, spawnDistance + spawnVariation);

            // Ajoutez une variation lat�rale al�atoire � la position de spawn.
            float randomLateralOffset = Random.Range(-lateralSpawnVariation, lateralSpawnVariation);
            Vector3 spawnPosition = path.position + path.forward * randomSpawnDistance + path.right * randomLateralOffset;

            // Choisissez votre prefab d'ennemi � partir du tableau ou de votre r�f�rence unique.
            GameObject enemyPrefab = enemyPrefabs.Length > 0 ? enemyPrefabs[Random.Range(0, enemyPrefabs.Length)] : null;

            if (enemyPrefab != null)
            {
                GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                enemy.transform.LookAt(path.position + path.forward);

                // Ajoutez un script EnemyController (� cr�er) � l'ennemi pour g�rer sa destruction.
                EnemyController enemyController = enemy.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    enemyController.SetSpawner(this);
                }
            }
        }
    }

    bool PlayerEnteredSpawnZone()
    {
        // Assurez-vous que le collider d�clencheur est attach� � l'objet EnemySpawner et est configur� comme d�clencheur.
        Collider collider = GetComponent<Collider>();
        if (collider != null && collider.isTrigger)
        {
            // V�rifiez si le joueur est � l'int�rieur du collider d�clencheur.
            return collider.bounds.Contains(GameObject.FindGameObjectWithTag("Player").transform.position);
        }

        return false;
    }

    void DestroyFarEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            // V�rifiez si l'ennemi existe toujours (il pourrait �tre d�truit par ailleurs).
            if (enemy != null)
            {
                // Calculez la distance entre le joueur et l'ennemi.
                float distanceToPlayer = Vector3.Distance(enemy.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);

                // D�truisez l'ennemi si la distance d�passe le seuil maximal.
                if (distanceToPlayer > maxDistanceToPlayer)
                {
                    RemoveEnemy(enemy);
                }
            }
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        // Impl�mentez ici la logique pour retirer l'ennemi de votre liste ou effectuer d'autres actions n�cessaires.
        // Par exemple, vous pourriez le d�sactiver, le mettre en file d'attente pour r�utilisation, etc.
        Destroy(enemy);
    }
}

