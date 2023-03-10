using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;

public class Seleccionar_Personaje : MonoBehaviour
{
    private static string coneccion;
    private static string DB_ruta;
    string BD_nombre = "DBVideoJuego.db";
    //private IDbConnection DB_coneccion;

    IDbConnection DB_coneccion;
    IDbCommand DB_comando;
    IDataReader leer_datos;

    public Button[] btn_Personajes;
    public Image imagen_Descripcion_Personaje;
    public TextMeshProUGUI texto_Descripcion_Personaje;

    public void Start()
    {
        float cant_personajes_habilitar = 100 / btn_Personajes.Length;
        for (int i = 1; i < btn_Personajes.Length; i++)
        {
            if (cant_personajes_habilitar * i > Datos_Slot.porcentaje_Partida_Gloval)
            {
                btn_Personajes[i].interactable = false;
            }
        }
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

    public void imagen_seleccionar_Personaje(Image Imagen_Personaje)
    {
        imagen_Descripcion_Personaje.sprite = Imagen_Personaje.sprite;
    }

    public void codigo_seleccionar_Personaje(string Cod_Personaje)
    {
        inicializar_coneccion();
        string sqlQuery;

        DB_comando = DB_coneccion.CreateCommand();
        sqlQuery = "SELECT IB.Descripcion_Personaje FROM Personaje P INNER JOIN Informacion_Basica IB ON P.Cod_Informacion_Basica = IB.Cod_Informacion_Basica WHERE P.Cod_Personaje = '" + Cod_Personaje +"'";
        DB_comando.CommandText = sqlQuery;
        leer_datos = DB_comando.ExecuteReader();
        leer_datos.Read();

        texto_Descripcion_Personaje.text = leer_datos.GetString(0);

        leer_datos.Close();
        leer_datos = null;

        cerrar_coneccion();
    }
}
