using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;
using System;
using UnityEngine.UI;

public class BD_Partida : MonoBehaviour
{
    private static string coneccion;
    private static string DB_ruta;
    string BD_nombre = "DBVideoJuego.db";

    IDbConnection DB_coneccion;
    IDbCommand DB_comando;
    IDataReader leer_datos;

    public GameObject slot;
    private int numero_Filas;

    void Start()
    {
        cantidad_slots();
        inicializar_Slots();
    }

    public void inicializar_coneccion()
    {
        DB_ruta = Application.dataPath + "/StreamingAssets/" + BD_nombre;
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            DB_ruta = Application.dataPath + "/StreamingAssets/" + BD_nombre;
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            DB_ruta = Application.dataPath + "/Raw/" + BD_nombre;
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            DB_ruta = Application.persistentDataPath + "/" + BD_nombre;
            if (!File.Exists(DB_ruta))
            {
                WWW DB_load = new WWW("jar:file://" + Application.dataPath + "!/assets/" + BD_nombre);
                while (!DB_load.isDone)
                {

                }
                File.WriteAllBytes(DB_ruta, DB_load.bytes);
            }
        }
        coneccion = "URI=file:" + DB_ruta;

        DB_coneccion = new SqliteConnection(coneccion);
        DB_coneccion.Open();
    }

    public void cerrar_coneccion()
    {
        DB_comando.Dispose();
        DB_comando = null;
        DB_coneccion.Close();
        DB_coneccion = null;
    }

    public void cantidad_slots()
    {
        inicializar_coneccion();

        DB_comando = DB_coneccion.CreateCommand();
        string sqlQuery = "SELECT count(*) " + "FROM Partida";
        DB_comando.CommandText = sqlQuery;
        leer_datos = DB_comando.ExecuteReader();
        leer_datos.Read();

        numero_Filas = leer_datos.GetInt32(0);

        leer_datos.Close();
        leer_datos = null;

        cerrar_coneccion();
    }

    public void inicializar_Slots()
    {
        inicializar_coneccion();

        DB_comando = DB_coneccion.CreateCommand();
        string sqlQuery = "SELECT Cod_Partida " + "FROM Partida";
        DB_comando.CommandText = sqlQuery;
        leer_datos = DB_comando.ExecuteReader();

        Vector3 posicion_inicial = this.transform.localPosition;
        posicion_inicial.y = 100;

        for (int i = 0; i < numero_Filas; i++)
        {
            leer_datos.Read();
            GameObject instanciarSlot = Instantiate(slot, posicion_inicial, Quaternion.identity) as GameObject;
            instanciarSlot.name = leer_datos.GetInt16(0).ToString();
            instanciarSlot.transform.SetParent(this.transform);
            instanciarSlot.transform.localScale = this.transform.localScale;
            instanciarSlot.transform.localPosition = posicion_inicial;
            posicion_inicial.y -= 60;
        }

        leer_datos.Close();
        leer_datos = null;

        cerrar_coneccion();
    }
}
