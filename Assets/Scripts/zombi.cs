using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class zombi : MonoBehaviour
{
    // Variables de movimiento y salto
    private float moveSpeed = 8f;
    private float jumpForce = 8f;

    // Componentes
    private Rigidbody2D rb;
    private Animator animator;

    // Verificaci�n de suelo
    private bool isGrounded = false;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    // Variables de l�mites de pantalla
    private float minX, maxX;
    private float halfWidth;

    // Variables de orientaci�n
    private bool mirandodercha = true;

    // Variables de vida
    public int vida = 5;
    public Slider barraDeVida; // Asignar en el inspector

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Calcular la mitad del ancho del personaje
        halfWidth = GetComponent<SpriteRenderer>().bounds.extents.x;

        // Calcular los l�mites de la pantalla
        Camera cam = Camera.main;
        float screenHalfWidthInWorldUnits = cam.aspect * cam.orthographicSize;
        minX = -screenHalfWidthInWorldUnits + halfWidth;
        maxX = screenHalfWidthInWorldUnits - halfWidth;

        // Inicializar la barra de vida
        if (barraDeVida != null)
        {
            barraDeVida.maxValue = vida;
            barraDeVida.value = vida;
        }
    }

    void Update()
    {
        // Movimiento de izquierda a derecha
        float moveInput = Input.GetAxis("Horizontal");

        // Procesar animaciones y orientaci�n
        ProcesarMovimiento(moveInput);

        // Salto
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // Limitar la posici�n del personaje en el eje X
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        transform.position = clampedPosition;
    }

    void FixedUpdate()
    {
        // Verificar si est� en el suelo o en un cubo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void ProcesarMovimiento(float inputMovimiento)
    {
        // Animaciones
        if (inputMovimiento != 0f)
        {
            animator.SetBool("isRunnig", true);
            rb.velocity = new Vector2(inputMovimiento * moveSpeed, rb.velocity.y);
        }
        else
        {
            animator.SetBool("isRunnig", false);
        }

        // Orientaci�n
        GestionarOrientacion(inputMovimiento);
    }

    private void GestionarOrientacion(float inputMovimiento)
    {
        if ((mirandodercha == true && inputMovimiento < 0) || (mirandodercha == false && inputMovimiento > 0))
        {
            mirandodercha = !mirandodercha;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
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

    // M�todo para reducir la vida
    public void ReducirVida(int cantidad)
    {
        vida -= cantidad;
        if (barraDeVida != null)
        {
            barraDeVida.value = vida;
        }

        if (vida <= 0)
        {
            SceneManager.LoadScene("GameOver"); // Aseg�rate de que esta escena exista
        }
    }
}
