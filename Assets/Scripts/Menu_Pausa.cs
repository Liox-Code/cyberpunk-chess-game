using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Pausa : MonoBehaviour
{
    public static bool Juego_Esta_Pausado = false;
    public GameObject menu_Pausa_UI;

    public void continuar_Juego()
    {
        menu_Pausa_UI.SetActive(false);
        Time.timeScale = 1f;
        Juego_Esta_Pausado = false;
    }

    public void pausar_Juego()
    {
        menu_Pausa_UI.SetActive(true);
        Time.timeScale = 0f;
        Juego_Esta_Pausado = true;
    }

    public void Menu_Partida()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu_Partida");
    }
}
