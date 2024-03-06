using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesCollect : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            ScoreManager.scoreCount += 1;
        }
    }
}