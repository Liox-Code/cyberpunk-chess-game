using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;

public class Ganar_Nivel : MonoBehaviour
{
    public static Ganar_Nivel instancia { get; set; }

    private static string coneccion;
    private static string DB_ruta;
    string BD_nombre = "DBVideoJuego.db";
    private IDbConnection DB_coneccion;
    private IDbCommand DB_comando;
    private IDataReader leer_datos;

    public void Start()
    {
        instancia = this;
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

    public void Aumentar_Porcentaje_Partida(float porcentaje_Actualizado)
    {
        inicializar_coneccion();

        DB_comando = DB_coneccion.CreateCommand();
        string sqlQuery = string.Format("UPDATE Partida SET Porcemtaje_Partida = {0} WHERE Cod_Partida = {1}", porcentaje_Actualizado, Datos_Slot.numero_Partida_Gloval);
        DB_comando.CommandText = sqlQuery;
        DB_comando.ExecuteNonQuery();
        

        sqlQuery = string.Format("UPDATE Progreso SET Completo = {0} WHERE Cod_Nivel_Juego = \"{1}\" AND Cod_Partida = \"{2}\"", (Seleccionar_Nivel.completado ? 1 : 0), Datos_Slot.Nivel_Actual, Datos_Slot.numero_Partida_Gloval);
        DB_comando.CommandText = sqlQuery;
        DB_comando.ExecuteNonQuery();

        cerrar_coneccion();
    }
}
