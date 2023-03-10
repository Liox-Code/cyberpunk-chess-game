using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tableroRangoMovimientos : MonoBehaviour
{
    public static tableroRangoMovimientos instancia { get; set; }

    public GameObject rangoCuadrosPrefabMovimiento;
    public GameObject rangoCuadrosPrefabAtaque;
    private List<GameObject> rangoCuadros;
    private List<GameObject> rangoCuadrosAtaque;

    private void Start()
    {
        instancia = this;
        rangoCuadros = new List<GameObject>();
        rangoCuadrosAtaque = new List<GameObject>();

    }

    public void rangoCuadrosPermitidos(bool[,] movimientos)
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (movimientos[i, j] == true)
                {
                    GameObject go;
                    go = getRangoCuadrosObjeto();
                    go.SetActive(true);
                    go.transform.position = new Vector3((i*3) + 1.5f, j + 0.5f, 0);
                }
            }
        }
    }


    private GameObject getRangoCuadrosObjeto()
    {
        GameObject go = null;
        if (Rango_Ataque_Movimiento.instancia.Accion_Movimiento == true)
        {
            go = rangoCuadros.Find(g => !g.activeSelf);
        }
        if (Rango_Ataque_Movimiento.instancia.Accion_Ataque == true)
        {
            go = rangoCuadrosAtaque.Find(g => !g.activeSelf);
        }
        if (go == null)
        {
            if (Rango_Ataque_Movimiento.instancia.Accion_Movimiento == true)
            {
                go = Instantiate(rangoCuadrosPrefabMovimiento);
                rangoCuadros.Add(go);
            }
            if (Rango_Ataque_Movimiento.instancia.Accion_Ataque == true)
            {
                go = Instantiate(rangoCuadrosPrefabAtaque);
                rangoCuadrosAtaque.Add(go);
            }
        }
        return go;
    }

    public void ocultarRangoCuadros()
    {
        if (Rango_Ataque_Movimiento.instancia.Accion_Movimiento == true)
        {
            foreach (GameObject go in rangoCuadros)
            {
                go.SetActive(false);
            }
        }
        if (Rango_Ataque_Movimiento.instancia.Accion_Ataque == true)
        {
            foreach (GameObject go in rangoCuadrosAtaque)
            {
                go.SetActive(false);
            }
        }
    }
}
