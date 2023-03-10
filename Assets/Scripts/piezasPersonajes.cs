using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class piezasPersonajes : MonoBehaviour
{
    public float Vida_Maxima = 100;
    [SerializeField]
    public float Vida_Actual { get; set; }
    public float Daño_Maximo = 20;
    public float daño { get; set; }

    public bool muerto { get; set; }

    public int posicionActualX { set; get; }
    public int posicionActualY { set; get; }
    public bool esAliado;

    public void setPosicion(int x, int y) {
        posicionActualX = x;
        posicionActualY = y;
    }

    public virtual bool[,] posibleMovimiento()
    {
        return new bool[6, 4];
    }

    public virtual bool[,] posibleAtaque()
    {
        return new bool[6, 4];
    }

    public virtual void Daño_Recibido()
    {
        Debug.Log("Daño 0");
    }
}
