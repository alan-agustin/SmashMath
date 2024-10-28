using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubitos : MonoBehaviour
{

    private float _vel;

    private Vector2 minPantalla;




    void Start()
    {
        _vel = 5f;

        minPantalla = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));





    }

    void Update()
    {

        Vector2 novaPos = transform.position;
        novaPos += new Vector2(0, -1) * _vel * Time.deltaTime;
        transform.position = novaPos;

    }
}
