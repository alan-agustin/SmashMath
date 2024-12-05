using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoPersonaje : MonoBehaviour
{

    public float velocidad;
    public int vida = 5;

    private Rigidbody2D rigidBody;
    private bool mirandodercha = true;
    private Animator animator;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
    }



    // Update is called once per frame
    void Update()
    {
       ProcesarMovimiento();
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            // Reduce la variable
            vida--;

        }*/

        void ProcesarMovimiento()
        {
            float inputMovimiento = Input.GetAxis("Horizontal");

            if (inputMovimiento != 0f)
            {
                animator.SetBool("isRunnig", true);
            }
            else
            {
                animator.SetBool("isRunnig", false);
            }


            rigidBody.velocity = new Vector2(inputMovimiento * velocidad, rigidBody.velocity.y);

            GestionarOrientacion(inputMovimiento);
        }

        void GestionarOrientacion(float inputMovimiento)
        {
            if ((mirandodercha == true && inputMovimiento < 0) || (mirandodercha == false && inputMovimiento > 0))
            {
                mirandodercha = !mirandodercha;
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            }
        }
    }
}
