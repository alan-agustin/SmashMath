using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BarraVida : MonoBehaviour
{
    public Image rellenoBarraVida;
    private MovimientoPersonaje playerController;
    private float vidaMaxima;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("personaje").GetComponent<MovimientoPersonaje>();
        vidaMaxima = playerController.vida;
    }

    // Update is called once per frame
    void Update()
    {
        rellenoBarraVida.fillAmount = playerController.vida / vidaMaxima;
    }
}
