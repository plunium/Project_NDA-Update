using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemySpawner spawner;

    public void SetSpawner(EnemySpawner _spawner)
    {
        spawner = _spawner;
    }

    void OnTriggerEnter(Collider other)
    {
        // D�truire l'ennemi lorsque le joueur passe devant.
        if (other.CompareTag("PlayerArmature"))
        {
            DestroyEnemy();
        }
    }

    void OnBecameInvisible()
    {
        // D�truire l'ennemi lorsqu'il n'est plus visible par la cam�ra.
        DestroyEnemy();
    }

    void DestroyEnemy()
    {
        // Informer le spawner que l'ennemi doit �tre retir� de sa liste.
        if (spawner != null)
        {
            spawner.RemoveEnemy(gameObject);
        }
    }
}

