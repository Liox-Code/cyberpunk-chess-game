using UnityEngine;
using UnityEngine.UI;

public class Personaje_Mid_Robot : piezasPersonajes
{
    public Image img_Barra_Vida;

    public void Awake()
    {
        Vida_Actual = Vida_Maxima;
        daño = 5;
    }

    public override void Daño_Recibido()
    {
        Vida_Actual -= daño;
        img_Barra_Vida.fillAmount = Vida_Actual / Vida_Maxima;
        if (Vida_Actual <= 0)
        {
            Morir();
        }
    }

    public void Morir()
    {
        muerto = true;
    }

    public override bool[,] posibleMovimiento()
    {
        bool[,] r = new bool[6, 4];

        piezasPersonajes c1;
        if (posicionActualX != 0)
        {
            c1 = crearTablero.instancia.personajePieza[posicionActualX - 1, posicionActualY];
            if (c1 == null)
            {
                r[posicionActualX - 1, posicionActualY] = true;
            }
        }

        if (posicionActualX != 5)
        {
            c1 = crearTablero.instancia.personajePieza[posicionActualX + 1, posicionActualY];
            if (c1 == null)
            {
                r[posicionActualX + 1, posicionActualY] = true;
            }
        }

        if (posicionActualY != 0)
        {
            c1 = crearTablero.instancia.personajePieza[posicionActualX, posicionActualY - 1];
            if (c1 == null)
            {
                r[posicionActualX, posicionActualY - 1] = true;
            }
        }

        if (posicionActualY != 3)
        {
            c1 = crearTablero.instancia.personajePieza[posicionActualX, posicionActualY + 1];
            if (c1 == null)
            {
                r[posicionActualX, posicionActualY + 1] = true;
            }
        }

        if (posicionActualX != 0 && posicionActualY != 0)
        {
            c1 = crearTablero.instancia.personajePieza[posicionActualX - 1, posicionActualY - 1];
            if (c1 == null)
            {
                r[posicionActualX - 1, posicionActualY - 1] = true;
            }
        }

        if (posicionActualX != 5 && posicionActualY != 3)
        {
            c1 = crearTablero.instancia.personajePieza[posicionActualX + 1, posicionActualY + 1];
            if (c1 == null)
            {
                r[posicionActualX + 1, posicionActualY + 1] = true;
            }
        }

        if (posicionActualX != 0 && posicionActualY != 3)
        {
            c1 = crearTablero.instancia.personajePieza[posicionActualX - 1, posicionActualY + 1];
            if (c1 == null)
            {
                r[posicionActualX - 1, posicionActualY + 1] = true;
            }
        }

        if (posicionActualX != 5 && posicionActualY != 0)
        {
            c1 = crearTablero.instancia.personajePieza[posicionActualX + 1, posicionActualY - 1];
            if (c1 == null)
            {
                r[posicionActualX + 1, posicionActualY - 1] = true;
            }
        }

        //if (posicionActualX != 0 && posicionActualY != 7)
        //{
        //    c = crearTablero.instancia.personajePieza[posicionActualX - 1, posicionActualY + 1];
        //    if (c != null)
        //    {
        //        r[posicionActualX - 1, posicionActualY + 1] = true;
        //    }
        //}

        //if (posicionActualX != 7 && posicionActualY != 7)
        //{
        //    c = crearTablero.instancia.personajePieza[posicionActualX + 1, posicionActualY + 1];
        //    if (c != null)
        //    {
        //        r[posicionActualX + 1, posicionActualY + 1] = true;
        //    }
        //}

        //if (posicionActualY != 7)
        //{
        //    c = crearTablero.instancia.personajePieza[posicionActualX, posicionActualY + 1];
        //    if (c == null)
        //    {
        //        r[posicionActualX, posicionActualY + 1] = true;
        //    }
        //}

        //if (posicionActualY == 1)
        //{
        //    c = crearTablero.instancia.personajePieza[posicionActualX, posicionActualY + 1];
        //    c2 = crearTablero.instancia.personajePieza[posicionActualX, posicionActualY + 2];
        //    if (c == null && c2 == null)
        //    {
        //        r[posicionActualX, posicionActualY + 2] = true;
        //    }
        //}

        return r;
    }

    public override bool[,] posibleAtaque()
    {
        bool[,] r = new bool[8, 8];

        piezasPersonajes c1;

        if (posicionActualX != 0)
        {
            c1 = crearTablero.instancia.personajePieza[posicionActualX - 1, posicionActualY];
            r[posicionActualX - 1, posicionActualY] = true;
        }
        if (posicionActualX != 0 && posicionActualX != 1)
        {
            c1 = crearTablero.instancia.personajePieza[posicionActualX - 2, posicionActualY];
            r[posicionActualX - 2, posicionActualY] = true;
        }
        if (posicionActualX != 0 && posicionActualX != 1 && posicionActualX != 2)
        {
            c1 = crearTablero.instancia.personajePieza[posicionActualX - 3, posicionActualY];
            r[posicionActualX - 3, posicionActualY] = true;
        }
        return r;
    }
}
