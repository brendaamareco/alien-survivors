using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScript : MonoBehaviour
{
    public float velocidadX = 0.05f; // Velocidad de movimiento en el eje X
    public bool ejeX = true;
    public bool sumar = true;

    private float tiempoAnterior; // Almacena el tiempo en el que se realizó la última actualización

    private void Start()
    {
        tiempoAnterior = Time.time; // Inicializa el tiempo anterior al tiempo actual
    }

    private void Update()
    {
        // Calcula el tiempo que ha pasado desde la última actualización
        float tiempoTranscurrido = Time.time - tiempoAnterior;

        // Si ha pasado al menos 1 segundo, realiza la operación de movimiento
        if (tiempoTranscurrido >= 1.0f)
        {
            // Calcula la cantidad de movimiento en el eje X
            float movimientoEje = velocidadX * tiempoTranscurrido;

            if (ejeX && sumar)
            {
                // Suma la cantidad de movimiento al valor actual de la posición X
                transform.position += new Vector3(movimientoEje, 0.0f, 0.0f);
            }
            else if (ejeX && !sumar)
            {
                transform.position -= new Vector3(movimientoEje, 0.0f, 0.0f);
            }
            else if (!ejeX && sumar)
            {
                transform.position += new Vector3(0.0f, 0.0f, movimientoEje);
            }
            else
            {
                transform.position -= new Vector3(0.0f, 0.0f, movimientoEje);

            }

            // Actualiza el tiempo anterior al tiempo actual
            tiempoAnterior = Time.time;
        }
    }
}
