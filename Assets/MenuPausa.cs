using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{

    [SerializeField] private GameObject botonPausa;
    [SerializeField] private GameObject menuPausa;
    public GameObject pauseMenuUI; // Arrastra el Canvas de tu menú aquí desde el Inspector

    private bool isPaused = false;

    void Update()
    {
        // Detecta si se presiona la tecla Esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false); // Oculta el menú
        Time.timeScale = 1f;         // Reanuda el tiempo del juego
        isPaused = false;
    }

    // Pausar el juego
    void Pause()
    {
        pauseMenuUI.SetActive(true); // Muestra el menú
        Time.timeScale = 0f;        // Pausa el tiempo del juego
        isPaused = true;
    }
    public void Pausa()
    {
        Time.timeScale = 0f;
        botonPausa.SetActive(false);
        menuPausa.SetActive(true);
    }

    public void Reaunudar()
    {
        Time.timeScale = 1f;
        botonPausa.SetActive(true);
        menuPausa.SetActive(false);
    }

    public void Reiniciar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Salir()
    {
        //Ir a pagina Principal
        SceneManager.LoadScene("Principal");
    }
}
