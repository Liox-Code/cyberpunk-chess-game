using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cambiar_Escena : MonoBehaviour
{
    public void cargar_escena_menu_principal()
    {
        SceneManager.LoadScene("Menu_Principal");
    }

    public void cargar_escena_menu_partida()
    {
        if (Datos_Slot.numero_Partida_Gloval != 0)
        {
            SceneManager.LoadScene("Menu_Partida");
        }
    }

    public void Salir()
    {
        Application.Quit();
    }

    public void cambiar_Escena_Juego(string Nombre_Escena)
    {
        SceneManager.LoadScene(Nombre_Escena);
    }
}
