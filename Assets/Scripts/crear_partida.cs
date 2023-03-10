using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

using TMPro;

public class crear_partida : MonoBehaviour
{
    private static string DB_ruta;
    private static string coneccion;

    IDbConnection DB_coneccion;
    IDbCommand DB_comando;
    IDataReader leer_datos;

    string BD_nombre = "DBVideoJuego.db";

    public InputField texto_Nombre_Partida;

    void Start()
    {
    }

    public void inicializar_coneccion()
    {
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

    public int cantidad_slots()
    {
        int numero_Filas;
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
        return numero_Filas;
    }

    public void nueva_Partida()
    {

        string sqlQuery;
        int numero_Filas = cantidad_slots();
        int cod_Partida;

        if (numero_Filas < 4)
        {
            inicializar_coneccion();
            DB_comando = DB_coneccion.CreateCommand();
            sqlQuery = string.Format("INSERT INTO Partida VALUES (NULL ,'{0}',0)", texto_Nombre_Partida.text) ;
            DB_comando.CommandText = sqlQuery;
            DB_comando.ExecuteNonQuery();
            cerrar_coneccion();

            insertar_Nivel_Partida();
            Debug.Log("Partida Creada");

        }
        else
        {
            Debug.Log("No tiene mas espacio");
        }
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("Menu_Partida");
    }

    public void insertar_Nivel_Partida()
    {
        string sqlQuery;
        int cod_Partida = 1;
        inicializar_coneccion();
        DB_comando = DB_coneccion.CreateCommand();
        sqlQuery = string.Format("SELECT Cod_Partida FROM Partida WHERE Nombre_Partida = '{0}'", texto_Nombre_Partida.text);
        DB_comando.CommandText = sqlQuery;
        leer_datos = DB_comando.ExecuteReader();
        leer_datos.Read();
        cod_Partida = leer_datos.GetInt32(0);
        leer_datos.Close();
        leer_datos = null;

        DB_comando = DB_coneccion.CreateCommand();
        sqlQuery = string.Format("INSERT INTO Progreso VALUES ({0},'Nivel_01',0);", cod_Partida);
        DB_comando.CommandText = sqlQuery;
        DB_comando.ExecuteNonQuery();
        DB_comando = DB_coneccion.CreateCommand();
        sqlQuery = string.Format("INSERT INTO Progreso VALUES ({0},'Nivel_02',0);", cod_Partida);
        DB_comando.CommandText = sqlQuery;
        DB_comando.ExecuteNonQuery();
        DB_comando = DB_coneccion.CreateCommand();
        sqlQuery = string.Format("INSERT INTO Progreso VALUES ({0},'Nivel_03',0);", cod_Partida);
        DB_comando.CommandText = sqlQuery;
        DB_comando.ExecuteNonQuery();


        Datos_Slot.numero_Partida_Gloval = cod_Partida;
        Datos_Slot.porcentaje_Partida_Gloval = 0;

        cerrar_coneccion();
    }
}
