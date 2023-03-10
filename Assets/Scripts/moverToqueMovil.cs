using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moverToqueMovil : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.touchCount > 0)
        //{
        //    Touch touch = Input.GetTouch(0);
        //    Vector3 posicionTouch = Camera.main.ScreenToWorldPoint(touch.position);
        //    posicionTouch.z = 0;
        //    transform.position = posicionTouch;

        //    Debug.Log("Posicion Pixeles" + touch.position);
        //    Debug.Log("Posicion Camara" + posicionTouch);
        //}

        /*for (int i=0; i < Input.touchCount; i++) {
            Vector3 posicionToque = Camera.main.ScreenToWorldPoint(Input.touches[i].position);
            Debug.DrawLine(Vector3.zero,posicionToque,Color.red);
            Debug.Log(" "+ Input.touches[i].position);
        }*/
    }
}
