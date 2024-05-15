using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMenuScene : MonoBehaviour
{
    // Mettez à jour est appelé une fois par image
    void Update()
    {
        // Vérifiez si la touche Échap a été pressée
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Chargez la scène du menu
            SceneManager.LoadScene("MainMenu");
        }
    }
}

