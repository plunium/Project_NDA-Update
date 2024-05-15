using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMenuScene : MonoBehaviour
{
    // Mettez � jour est appel� une fois par image
    void Update()
    {
        // V�rifiez si la touche �chap a �t� press�e
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Chargez la sc�ne du menu
            SceneManager.LoadScene("MainMenu");
        }
    }
}

