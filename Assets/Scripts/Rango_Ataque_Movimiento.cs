using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rango_Ataque_Movimiento : MonoBehaviour
{
    public static Rango_Ataque_Movimiento instancia { get; set; }
    public bool Accion_Movimiento  = true;
    public bool Accion_Ataque = false;

    public GameObject btn_primer_ataque_habilidad;
    public GameObject btn_segundo_ataque_habilidad;
    public GameObject img_bar_ataque;

    public Button btn_Mover;
    public Button btn_Atacar;
    public Button btn_Pasar_Turno;
    // Start is called before the first frame update
    void Start()
    {
        instancia = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void rangoAtaque()
    {
        tableroRangoMovimientos.instancia.ocultarRangoCuadros();
        Accion_Movimiento = false;
        Accion_Ataque = true;
        crearTablero.instancia.personajeSeleccionado = null;
    }

    public void rangoMovimiento()
    {
        btn_primer_ataque_habilidad.SetActive(false);
        btn_segundo_ataque_habilidad.SetActive(false);
        tableroRangoMovimientos.instancia.ocultarRangoCuadros();
        Accion_Movimiento = true;
        Accion_Ataque = false;
        crearTablero.instancia.personajeSeleccionado = null;
    }
}
