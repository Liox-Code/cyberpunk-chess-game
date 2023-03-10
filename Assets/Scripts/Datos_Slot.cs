using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;
using System;
using TMPro;
using System.IO;

public class Datos_Slot : MonoBehaviour
{
    public TextMeshProUGUI txt_nombre_Partida;
    public TextMeshProUGUI txt_porcentaje_Partida;
    public Button btn_jugar;
    public Button btn_eliminar;

    private static string coneccion;
    private static string DB_ruta;
    string BD_nombre = "DBVideoJuego.db";

    private IDbConnection DB_coneccion;
    private IDbCommand DB_comando;
    private IDataReader leer_datos;

    public static int numero_Partida_Gloval { get; set; }
    public static float porcentaje_Partida_Gloval { get; set; }
    public static float porcentaje_Nivel_Completado { get; set; }
    public static string Nivel_Actual { get; set; }

    private int numero_Partida;
    private float porcentaje_Partida;

    void Start()
    {
        inicializar_coneccion();
        inicializar_Datos_Slot();
    }

    void Update()
    {

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

    public void inicializar_Datos_Slot()
    {
        inicializar_coneccion();

        DB_comando = DB_coneccion.CreateCommand();
        string sqlQuery = "SELECT Cod_Partida,Nombre_Partida,Porcemtaje_Partida " + "FROM Partida " + "WHERE Cod_Partida = " + this.gameObject.name;
        DB_comando.CommandText = sqlQuery;
        leer_datos = DB_comando.ExecuteReader();
        leer_datos.Read();

        numero_Partida = leer_datos.GetInt32(0);
        porcentaje_Partida = leer_datos.GetFloat(2);

        txt_nombre_Partida.text = leer_datos.GetString(1);
        txt_porcentaje_Partida.text = leer_datos.GetFloat(2).ToString() + "%";

        leer_datos.Close();
        leer_datos = null;

        cerrar_coneccion();
    }

    //Eliminar objeto slot
    //Eliminar datos slot de la Base de Datos
    public void eliminar_slot()
    {
        Destroy(this.gameObject);

        string sqlQuery;

        inicializar_coneccion();

        DB_comando = DB_coneccion.CreateCommand();
        sqlQuery = "DELETE FROM Partida WHERE Cod_Partida = " + numero_Partida;
        DB_comando.CommandText = sqlQuery;
        DB_comando.ExecuteNonQuery();

        sqlQuery = "DELETE FROM Progreso WHERE Cod_Partida = " + numero_Partida;
        DB_comando.CommandText = sqlQuery;
        DB_comando.ExecuteNonQuery();

        cerrar_coneccion();
    }

    //
    public void seleccionar_Partida()
    {
        numero_Partida_Gloval = numero_Partida;
        porcentaje_Partida_Gloval = porcentaje_Partida;
    }
}
