using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPositionScript : MonoBehaviour
{
    public float velocidadX = 0.05f; // Velocidad de movimiento en el eje X
    public bool ejeX = true;
    public bool sumar = true;

    public float umbralMinX = 137.0f;
    public float umbralMaxX = 12.0f;
    public float umbralMinZ = 172.0f;
    public float umbralMaxZ = 19.0f;

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
            float posicionX = transform.localPosition.x;
            float posicionZ = transform.localPosition.z;

            if (ejeX && sumar && posicionX < umbralMaxX)
            {
                transform.localPosition += new Vector3(movimientoEje, 0.0f, 0.0f);
            }
            else if (ejeX && !sumar && posicionX > umbralMinX)
            {
                transform.localPosition -= new Vector3(movimientoEje, 0.0f, 0.0f);
            }
            else if (!ejeX && sumar && posicionZ < umbralMaxZ)
            {
                transform.localPosition += new Vector3(0.0f, 0.0f, movimientoEje);
            }
            else if (!ejeX && !sumar && posicionZ > umbralMinZ)
            {
                transform.localPosition -= new Vector3(0.0f, 0.0f, movimientoEje);
            }

            // Actualiza el tiempo anterior al tiempo actual
            tiempoAnterior = Time.time;
        }
    }
}
