using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombi : MonoBehaviour
{
    private float moveSpeed = 8f; // Velocidad de movimiento del personaje
    private float jumpForce = 6f; // Fuerza del salto

    private Rigidbody2D rb;
    private bool isGrounded = false; // Para verificar si est� en el suelo
    public Transform groundCheck; // Objeto vac�o para detectar el suelo
    public float groundCheckRadius = 0.2f; // Radio del c�rculo de detecci�n
    public LayerMask groundLayer; // Capa del suelo y cubos

    private float minX, maxX; // L�mites de la pantalla en el eje X
    private float halfWidth; // Mitad del ancho del personaje

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Calcular la mitad del ancho del personaje
        halfWidth = GetComponent<SpriteRenderer>().bounds.extents.x;

        // Calcular los l�mites de la pantalla
        Camera cam = Camera.main;
        float screenHalfWidthInWorldUnits = cam.aspect * cam.orthographicSize;
        minX = -screenHalfWidthInWorldUnits + halfWidth;
        maxX = screenHalfWidthInWorldUnits - halfWidth;
    }

    void Update()
    {
        // Movimiento de izquierda a derecha
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Limitar la posici�n del personaje en el eje X
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        transform.position = clampedPosition;

        // Salto
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void FixedUpdate()
    {
        // Verificar si est� en el suelo o en un cubo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        // Dibujar un c�rculo en la escena para visualizar el �rea de verificaci�n del suelo
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("OnCollisionEnter2D triggered in Player with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Cube"))
        {
            // Aqu� cridarem el m�tode per generar i mostrar una operaci�
            Debug.Log("Col�lisi� amb un cub detectada en el PlayerController.");
            FindObjectOfType<MathOperationManager>().GenerateAndShowOperation();
        }
    }
}
