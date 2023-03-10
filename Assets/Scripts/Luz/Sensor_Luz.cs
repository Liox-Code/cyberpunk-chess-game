using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sensor_Luz : MonoBehaviour
{
    //Luz
    LightSensorPluginScript sensor_Luz;
    private SpriteRenderer dia_noche;
    private float tiempo = 5f;

    void Start()
    {
        //Luz
        sensor_Luz = GetComponent<LightSensorPluginScript>();
        dia_noche = GetComponent<SpriteRenderer>();
        dia_noche.color = new Color(1, 1, 1, 1);

    }

    void Update()
    {
        Dia_Noche();
    }

    public void Dia_Noche()
    {
        if (tiempo > 0)
        {
            tiempo -= Time.deltaTime;
        }
        else
        {
            tiempo = 5f;
            //Luz
            if (sensor_Luz.getLux() < 5)
            {
                dia_noche.color = new Color(0, 0.3009782f, 4, 0.4823529f);
            }
            else if (sensor_Luz.getLux() < 20)
            {
                dia_noche.color = new Color(1, 0.62403f, 0, 0.8392157f);
            }
            else
            {
                dia_noche.color = new Color(0.627451f, 0.9137255f, 0.9137255f, 1);
            }
        }
    }
}
