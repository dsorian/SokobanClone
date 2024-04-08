using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Scripting.APIUpdating;

public class Player : MonoBehaviour
{

    private float moveSpeed = 0.1f;
    private Rigidbody2D rb;
    private Vector2 movementDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Obtener las entradas de teclado
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
         // Calcular la dirección de movimiento
        Vector2 moveDirection = new Vector2(horizontalInput, verticalInput).normalized;

        // Limitar el movimiento a las cuatro direcciones cardinales
        if (moveDirection.x != 0 && moveDirection.y != 0)
        {
            // Si la entrada es diagonal, normalizar para mantener solo el movimiento en una dirección
            if (Mathf.Abs(horizontalInput) > Mathf.Abs(verticalInput))
            {
                moveDirection.y = 0;
            }
            else
            {
                moveDirection.x = 0;
            }
        }

        //movementDirection = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
        movementDirection = new Vector2(moveDirection.x,moveDirection.y);
    }

    void FixedUpdate(){
        //rb.velocity = movementDirection * moveSpeed;
        rb.MovePosition(rb.position+ movementDirection*moveSpeed);
    }
}
