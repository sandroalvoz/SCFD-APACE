using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Prueba : MonoBehaviour{
    // Se define la variable contador como un n�mero entero
    int contador = 0;
    void Start(){
        // Se mostrar� en la consola el siguiente texto:
        Debug.Log("El programa ha empezado a ejecutarse");
    }
    void Update(){
        contador++;
        //a cada vuelta del bucle Update(), contador aumenta una unidad
        if (contador == 1) {
            //Cuando contador sea igual a 1, se mostrar� el siguiente texto:
            Debug.Log("Se ha ejecutado un ciclo del bucle void Update ()");
        } else{
            //Si contador no es igual a 1, se mostrar� una cadena de texto:
            Debug.Log("Se han ejecutado " + contador + " ciclos del bucle void Update ()");
        }
    }
}