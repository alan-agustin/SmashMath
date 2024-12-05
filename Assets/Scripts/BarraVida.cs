using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    public Image rellenoBarraVida;
    private zombi playerController;
    private float vidaMaxima;

    void Start()
    {
        playerController = GameObject.Find("personaje").GetComponent<zombi>();
        if (playerController != null)
        {
            vidaMaxima = playerController.vida;
        }
        else
        {
            Debug.LogError("No se encontró el componente 'zombi' en el objeto 'personaje'. Asegúrate de que el nombre del objeto y el script sean correctos.");
        }
    }

    void Update()
    {
        if (playerController != null)
        {
            rellenoBarraVida.fillAmount = (float)playerController.vida / vidaMaxima;
        }
    }
}
