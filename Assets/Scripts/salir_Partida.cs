using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class salir_Partida : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void reiniciar_datos_partida()
    {
        Datos_Slot.numero_Partida_Gloval = 0;
        Datos_Slot.porcentaje_Partida_Gloval = 0;
    }
}
