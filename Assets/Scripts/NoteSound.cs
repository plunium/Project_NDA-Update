using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectNote : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        // Assurez-vous d'avoir une source audio sur le même objet que ce script
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        // Vérifiez si le joueur a ramassé la note de musique
        if (collision.gameObject.tag == "Player")
        {
            // Activez l'AudioSource et jouez le son
            audioSource.Play();
            

          
        }
    }
}
