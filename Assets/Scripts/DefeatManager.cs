using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 


public class DefeatManager : MonoBehaviour
{

    public void VolverAJugar()
    {
       
        Debug.Log("Reiniciando juego...");
        Time.timeScale = 1f;
        SceneManager.LoadScene("Juego");
    }
    public void VolverAlMenu()
    {
        
        Debug.Log("Volviendo al Menú Principal...");
        SceneManager.LoadScene("MainMenu");
    }

       public void SalirJuego()
    {
        Debug.Log("Saliendo del juego...");

        

#if UNITY_EDITOR
            
            UnityEditor.EditorApplication.isPlaying = false;
#else
        
        Application.Quit();
#endif
    }


}