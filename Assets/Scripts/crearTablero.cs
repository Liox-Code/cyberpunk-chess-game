using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.SceneManagement;

public class crearTablero : MonoBehaviour
{
    public static crearTablero instancia { get; set; }
    private bool[,] movimientosPermitidos { get; set; }
    private bool[,] rangoAtaquePermitidos { get; set; }

    public piezasPersonajes[,] personajePieza { set; get; }
    public piezasPersonajes personajeSeleccionado;

    private const float ancho_cuadro = 3f;
    private const float alto_cuadro = 1f;
    private const float fondo_cuadro = 0.4f;

    private const float movimiento_x = 1.5f;
    private const float movimiento_y = 0.5F;
    private const float movimiento_z = 0.1f;

    private int posicionSeleccionadaX = -1;
    private int posicionSeleccionadaY = -1;

    private Quaternion orientacionPieza = Quaternion.Euler(0, 0, 0);

    public List<GameObject> piezasPersonajesPrefabs;
    private List<GameObject> piezasPersonajesActivos = new List<GameObject>();

    //audio
    public Image img_Barra_Ataque;
    private AudioSource AS_Sensor_Audio;
    public AudioMixerGroup mixer_Microfono;
    private float sensividad = 1;
    private float ruido = 0;
    public TextMeshProUGUI daño_x_Sonido;

    private bool primer_ataque_activo = false;
    private bool segundo_ataque_activo = false;

    private float Daño_Total = 5;

    public bool Primer_Movimiento = false;
    public bool Turno_Aliado = true;

    private int Cantidad_Aliados = 0;
    private int Cantidad_Enemigos = 0;

    private void Start()
    {
        instancia = this;
        posicionarTodasPiezasPersonajes();
        Conteo_Aliados_Enemigos();
        Inicializar_Microfono();
    }

    public void Inicializar_Microfono()
    {
        //Audio
        AS_Sensor_Audio = GetComponent<AudioSource>();
        AS_Sensor_Audio.loop = true;
        if (Microphone.devices.Length > 0)
        {
            AS_Sensor_Audio.outputAudioMixerGroup = mixer_Microfono;
            AS_Sensor_Audio.clip = Microphone.Start(null, true, 10, AudioSettings.outputSampleRate);
        }
        while (!(Microphone.GetPosition(null) > 0))
        {
        }
        AS_Sensor_Audio.Play();
    }

    private void Update()
    {
        posicionSeleccionada();
        DibujarTablero();

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                if (posicionSeleccionadaX >= 0 && posicionSeleccionadaY >= 0 && posicionSeleccionadaX < 18 && posicionSeleccionadaY < 4)
                {
                    posicionSeleccionadaX = (((int)posicionSeleccionadaX / 3));
                    if (personajeSeleccionado == null)
                    {

                        if (Rango_Ataque_Movimiento.instancia.Accion_Movimiento == true)
                        {
                            if (!Primer_Movimiento)
                            {
                                selecionarPiezaPersonaje(posicionSeleccionadaX, posicionSeleccionadaY);
                            }
                        }
                        if (Rango_Ataque_Movimiento.instancia.Accion_Ataque == true)
                        {
                            selecionarPiezaPersonajeAtacar(posicionSeleccionadaX, posicionSeleccionadaY);
                        }
                    }
                    else
                    {
                        if (Rango_Ataque_Movimiento.instancia.Accion_Movimiento == true)
                        {
                            if (!Primer_Movimiento)
                            {
                                moverPiezaPersonaje(posicionSeleccionadaX, posicionSeleccionadaY);
                            }
                        }
                        if (Rango_Ataque_Movimiento.instancia.Accion_Ataque == true)
                        {
                            Rango_Ataque_Movimiento.instancia.btn_primer_ataque_habilidad.SetActive(false);
                            Rango_Ataque_Movimiento.instancia.btn_segundo_ataque_habilidad.SetActive(false);
                            tableroRangoMovimientos.instancia.ocultarRangoCuadros();
                            personajeSeleccionado = null;
                        }
                    }
                }
            }
        }

        if (primer_ataque_activo == true)
        {
            if ((get_Promedio_Volumen() * 100) > Daño_Total)
            {
                Daño_Total = get_Promedio_Volumen() * 100;
                daño_x_Sonido.text = ((int)(Daño_Total)).ToString();
            }
            img_Barra_Ataque.fillAmount = get_Promedio_Volumen() * 100;
        }
        if (segundo_ataque_activo == true)
        {
            if ((get_Promedio_Volumen() * 100) > Daño_Total)
            {
                Daño_Total = get_Promedio_Volumen() * 100;
                daño_x_Sonido.text = ((int)(Daño_Total)).ToString();
            }
            img_Barra_Ataque.fillAmount = get_Promedio_Volumen() * 100;
        }
    }

    private void posicionarTodasPiezasPersonajes()
    {
        personajePieza = new piezasPersonajes[6, 4];
        //Equi
        posicionarPiezaPersonaje(0, 0, 1);

        posicionarPiezaPersonaje(1, 0, 2);

        posicionarPiezaPersonaje(2, 5, 1);
        posicionarPiezaPersonaje(3, 5, 2);

    }

    private void Conteo_Aliados_Enemigos()
    {
        for (int x = 0; x < 6; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                piezasPersonajes c = personajePieza[x, y];
                if (c != null && personajePieza[x, y].esAliado == true)
                {
                    Cantidad_Aliados++;
                }
                else if (c != null && personajePieza[x, y].esAliado == false)
                {
                    Cantidad_Enemigos++;
                }
            }
        }
    }

    private void posicionarPiezaPersonaje(int numeroPieza, int x, int y)
    {
        GameObject instanciarPrefab = Instantiate(piezasPersonajesPrefabs[numeroPieza], getCentroCuadro(x, y), orientacionPieza) as GameObject;
        instanciarPrefab.transform.SetParent(this.transform);

        personajePieza[x, y] = instanciarPrefab.GetComponent<piezasPersonajes>();
        personajePieza[x, y].setPosicion(x, y);
        piezasPersonajesActivos.Add(instanciarPrefab);
    }

    private Vector3 getCentroCuadro(int x, int y)
    {
        Vector3 origen = Vector3.zero;
        origen.x += (ancho_cuadro * x) + movimiento_x;
        origen.y += (alto_cuadro * y) + movimiento_y;
        origen.z -= ((fondo_cuadro + 0.1f) - ((y+1) * 0.1f));
        return origen;
    }

    private void posicionSeleccionada()
    {
        if (!Camera.main)
            return;
        
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 posicionTouch = Camera.main.ScreenToWorldPoint(touch.position);
            posicionSeleccionadaX = (int)posicionTouch.x;
            posicionSeleccionadaY = (int)posicionTouch.y;
        }
        else
        {
            posicionSeleccionadaX = -1;
            posicionSeleccionadaY = -1;
        }
    }

    private void DibujarTablero()
    {
        if (posicionSeleccionadaX >= 0 && posicionSeleccionadaY >= 0 && posicionSeleccionadaX < 18 && posicionSeleccionadaY < 4)
        {
            Debug.DrawLine(
                Vector2.right * (((int)posicionSeleccionadaX / 3)*3) + Vector2.up * posicionSeleccionadaY,
                Vector2.right * ((((int)posicionSeleccionadaX / 3) * 3) + 3) + Vector2.up * (posicionSeleccionadaY + 1)
                );
            Debug.DrawLine(
                Vector2.right * ((((int)posicionSeleccionadaX / 3) * 3) + 3) + Vector2.up * posicionSeleccionadaY,
                Vector2.right * (((int)posicionSeleccionadaX / 3) * 3) + Vector2.up * (posicionSeleccionadaY + 1)
                );
        }
    }

    private void selecionarPiezaPersonaje(int x, int y)
    {
        if (personajePieza[x, y] == null)
        {
            return;
        }

        if (personajePieza[x, y].esAliado != Turno_Aliado)
        {
            return;
        }

        personajeSeleccionado = personajePieza[x, y];

        movimientosPermitidos = personajePieza[x, y].posibleMovimiento();
        tableroRangoMovimientos.instancia.rangoCuadrosPermitidos(movimientosPermitidos);
    }

    private void selecionarPiezaPersonajeAtacar(int x, int y)
    {
        if (personajePieza[x, y] == null)
        {
            return;
        }

        if (personajePieza[x, y].esAliado != Turno_Aliado)
        {
            return;
        }

        Rango_Ataque_Movimiento.instancia.btn_primer_ataque_habilidad.SetActive(true);
        Rango_Ataque_Movimiento.instancia.btn_segundo_ataque_habilidad.SetActive(true);
        personajeSeleccionado = personajePieza[x, y];

        rangoAtaquePermitidos = personajePieza[x, y].posibleAtaque();
        tableroRangoMovimientos.instancia.rangoCuadrosPermitidos(rangoAtaquePermitidos);
    }

    private void moverPiezaPersonaje(int x, int y)
    {
        if (movimientosPermitidos[x, y] == true)
        {
            piezasPersonajes c = personajePieza[x, y];
            if (c != null)
            {
                //Destruir pieza
                piezasPersonajesActivos.Remove(c.gameObject);
                Destroy(c.gameObject);
            }

            personajePieza[personajeSeleccionado.posicionActualX, personajeSeleccionado.posicionActualY] = null;
            personajeSeleccionado.transform.position = getCentroCuadro(x, y);
            personajeSeleccionado.setPosicion(x, y);
            personajePieza[x, y] = personajeSeleccionado;
            Primer_Movimiento = true;
            Rango_Ataque_Movimiento.instancia.btn_Mover.interactable = false;
        }

        tableroRangoMovimientos.instancia.ocultarRangoCuadros();
        personajeSeleccionado = null;
    }

    public void ataquePersonaje()
    {
        for (int x = 0; x < 6; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                if (rangoAtaquePermitidos[x, y] == true)
                {
                    piezasPersonajes c = personajePieza[x, y];
                    if (c != null && personajePieza[x, y].esAliado != Turno_Aliado)
                    {
                        //Destruir pieza
                        personajePieza[x, y].daño = Daño_Total;
                        personajePieza[x, y].Daño_Recibido();
                        personajePieza[x, y].daño = 5f;
                        if (personajePieza[x, y].muerto)
                        {
                            piezasPersonajesActivos.Remove(c.gameObject);
                            Destroy(c.gameObject);
                            Conteo_Personajes_Restantes(x,y);
                        }
                    }
                }
            }
        }

        Primer_Movimiento = false;
        Rango_Ataque_Movimiento.instancia.btn_Mover.interactable = true
            ;
        Turno_Aliado = !Turno_Aliado;
        Rango_Ataque_Movimiento.instancia.btn_primer_ataque_habilidad.SetActive(false);
        Rango_Ataque_Movimiento.instancia.btn_segundo_ataque_habilidad.SetActive(false);
        tableroRangoMovimientos.instancia.ocultarRangoCuadros();
        personajeSeleccionado = null;
    }

    private IEnumerator Atacar()
    {
        Rango_Ataque_Movimiento.instancia.btn_primer_ataque_habilidad.SetActive(false);
        Rango_Ataque_Movimiento.instancia.btn_segundo_ataque_habilidad.SetActive(false);
        Rango_Ataque_Movimiento.instancia.btn_Mover.interactable = false;
        Rango_Ataque_Movimiento.instancia.btn_Atacar.interactable = false;
        Rango_Ataque_Movimiento.instancia.btn_Pasar_Turno.interactable = false;
        Rango_Ataque_Movimiento.instancia.img_bar_ataque.SetActive(true);
        yield return new WaitForSeconds(3f);
        ataquePersonaje();
        if (primer_ataque_activo == true)
        {
            primer_ataque_activo = false;
        }
        if (segundo_ataque_activo == true)
        {
            segundo_ataque_activo = false;
        }
        Daño_Total = 5;
        daño_x_Sonido.text = ((int)(Daño_Total)).ToString();
        Rango_Ataque_Movimiento.instancia.btn_Mover.interactable = true;
        Rango_Ataque_Movimiento.instancia.btn_Atacar.interactable = true;
        Rango_Ataque_Movimiento.instancia.btn_Pasar_Turno.interactable = true;
        Rango_Ataque_Movimiento.instancia.img_bar_ataque.SetActive(false);
    }

    public void primer_ataque()
    {
        StartCoroutine(Atacar());
        primer_ataque_activo = true;
    }
    public void segundo_ataque()
    {
        StartCoroutine(Atacar());
        segundo_ataque_activo = true;
    }
    public float get_Promedio_Volumen()
    {
        float[] data = new float[1];
        float a = 0;
        AS_Sensor_Audio.GetOutputData(data, 0);
        a += Mathf.Abs(data[0]);
        //foreach (float s in data)
        //{
        //    a += Mathf.Abs(s);
        //}
        return a;
    }

    public void Pasar_Turno()
    {
        Rango_Ataque_Movimiento.instancia.btn_primer_ataque_habilidad.SetActive(false);
        Rango_Ataque_Movimiento.instancia.btn_segundo_ataque_habilidad.SetActive(false);
        tableroRangoMovimientos.instancia.ocultarRangoCuadros();
        Primer_Movimiento = false;
        Rango_Ataque_Movimiento.instancia.btn_Mover.interactable = true;

        personajeSeleccionado = null;
        Turno_Aliado = !Turno_Aliado;
    }
    
    private void Conteo_Personajes_Restantes(int x , int y)
    {
        if (personajePieza[x, y].esAliado == true)
        {
            Cantidad_Aliados--;
        }
        else
        {
            Cantidad_Enemigos--;
        }
        if (Cantidad_Aliados <= 0)
        {
            Debug.Log("Perdiste");
            SceneManager.LoadScene("Menu_Partida");

        }
        else if (Cantidad_Enemigos <= 0)
        {
            Debug.Log("Ganaste");
            if (Seleccionar_Nivel.completado == false)
            {
                Seleccionar_Nivel.completado = true;
                Datos_Slot.porcentaje_Partida_Gloval += Datos_Slot.porcentaje_Nivel_Completado;
                Ganar_Nivel.instancia.Aumentar_Porcentaje_Partida(Datos_Slot.porcentaje_Partida_Gloval);
            }
            SceneManager.LoadScene("Ganaste");
        }
    }

}
