using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;

public class Seleccionar_Nivel : MonoBehaviour
{
    public static Seleccionar_Nivel instancia { get; set; }
    public Button[] btn_Niveles;

    public static bool completado { get; set; }

    private static string coneccion;
    private static string DB_ruta;
    string BD_nombre = "DBVideoJuego.db";
    private IDbConnection DB_coneccion;
    private IDbCommand DB_comando;
    private IDataReader leer_datos;

    public void Start()
    {
        instancia = this;
        float cant_niveles_habilitar = 100 / btn_Niveles.Length;
        Datos_Slot.porcentaje_Nivel_Completado = cant_niveles_habilitar;
        for (int i = 1; i < btn_Niveles.Length; i++)
        {
            if (cant_niveles_habilitar * i > Datos_Slot.porcentaje_Partida_Gloval)
            {
                btn_Niveles[i].interactable = false;
            }
        }
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

    public void selector(string nombreNivel)
    {

        Datos_Slot.Nivel_Actual = nombreNivel;
        inicializar_coneccion();

        DB_comando = DB_coneccion.CreateCommand();
        string sqlQuery = string.Format("SELECT Completo FROM Progreso WHERE Cod_Nivel_Juego = \"{0}\" AND Cod_Partida = \"{1}\"", Datos_Slot.Nivel_Actual, Datos_Slot.numero_Partida_Gloval);
        DB_comando.CommandText = sqlQuery;
        leer_datos = DB_comando.ExecuteReader();
        leer_datos.Read();

        completado = leer_datos.GetBoolean(0);

        leer_datos.Close();
        leer_datos = null;

        cerrar_coneccion();

        SceneManager.LoadScene(nombreNivel);
    }

    public void Aumentar_Porcentaje_Partida()
    {
        Debug.Log("Nivel Actualizado");
        inicializar_coneccion();

        DB_comando = DB_coneccion.CreateCommand();
        string sqlQuery = string.Format("UPDATE Partida SET Porcemtaje_Partida = {0} WHERE Cod_Partida = {1}", Datos_Slot.porcentaje_Partida_Gloval, Datos_Slot.numero_Partida_Gloval);
        DB_comando.CommandText = sqlQuery;
        DB_comando.ExecuteNonQuery();

        cerrar_coneccion();
    }
}
