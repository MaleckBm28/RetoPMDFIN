using UnityEngine;
using UnityEngine.SceneManagement; 


public class MainMenuManager : MonoBehaviour
{
    public void Jugar()
    {

        string nombreDeEscena = "Juego";

        
        Debug.Log($"Cargando escena '{nombreDeEscena}'...");
        SceneManager.LoadScene(nombreDeEscena);
    }



    public void MostrarOpciones()
    {
        
        Debug.Log("Opc ha sido pulsado.");
        SceneManager.LoadScene("MenuOpciones");
    }

        public void MostrarCreditos()
    {
        Debug.Log("Creditos ha sido pulsado.");
        SceneManager.LoadScene("Creditos");
    }

    public void SalirJuego()
    {
        Debug.Log("Saliendo del juego...");


#if UNITY_EDITOR
            // Si estamos en el Editor, detenemos el modo "Play"
            UnityEditor.EditorApplication.isPlaying = false;
#else
        // Si estamos en una build (PC, Mac, etc.), cerramos la aplicaci�n
        Application.Quit();
#endif
    }
}
