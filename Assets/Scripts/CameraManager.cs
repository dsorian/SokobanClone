using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform target; // Referencia al transform del jugador

    public float smoothSpeed = 0.125f; // Velocidad de suavizado de movimiento de la cámara
    public Vector3 offset; // Desplazamiento adicional de la cámara respecto al jugador

    void FixedUpdate()
    {
        Vector3 desiredPosition;
        if (target != null)
        {
            // Calcula la posición objetivo de la cámara sumando el desplazamiento al jugador
            desiredPosition = target.position + offset;
        }else{
            desiredPosition = Input.mousePosition;
        }

                    // Interpola suavemente entre la posición actual de la cámara y la posición objetivo
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Actualiza la posición de la cámara
            transform.position = smoothedPosition;
    }
}
