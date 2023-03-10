using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;
using TMPro;
using System;
using System.Web;
using System.Text;
using System.Runtime.CompilerServices;
public class LlamadaAuxilio : MonoBehaviour{
    int Contador = 0;
    public int ContadorNotificacion = 0;
    public string Actualizacion;
    public float Latitud, Longitud;
    // Start is called before the first frame update
    void Start(){
        Debug.Log("Hola, documentación. El código se ha iniciado.");
        Input.location.Start();
}
    // Update is called once per frame
    void Update()
    {
        StartCoroutine(CorrutinaLectura("http://192.168.1.101:4072/SistemaLLA/EstadoAlarma"));
    }
    private IEnumerator CorrutinaLectura(string URL){
        UnityWebRequest request =UnityWebRequest.Get(URL);
        yield return request.SendWebRequest();
        //se envía una petición GET a través de UnityWebRequest
        if ((request.result != UnityWebRequest.Result.ConnectionError) && (request.result !=UnityWebRequest.Result.ProtocolError)) {
            //si la petición no falla, se continúa en la estructura if()
            //las siguientes lineas realizan una comparacion para determinar
            // si la alarma esta apagada o encendida.
            if (request.downloadHandler.text== "AlarmaApagada")
{
                Debug.Log("La alarma está apagada");
            }
            else
            {
                Debug.Log("La alarma está encendida");
            }
        } 
    }
    private IEnumerator CorrutinaSubida(string Nombrearchivo, string contenido){
        WWWForm form = new WWWForm();
        //se crea un formulario WWW
        form.AddField("archivo", Nombrearchivo);
        form.AddField("texto", contenido);
        //a este formulario, se le añade la informacion Nombrearchivo y contenido
        //estas variables se especificaran al llamar a la funcion desde el codigo
        using (UnityWebRequest www= UnityWebRequest.Post("http://192.168.1.101:4072/SistemaLLA/comunicacion.php", form)){
            //el formulario se envia a traves de una peticion POST al archivo comunicacion.php
            yield return www.SendWebRequest();
            //se envia la peticion web
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                //si la peticion falla, se muestra el error en la consola
            }
        }
    }
    public void Emision() {
        //----------------------------------------------------------------
        //CONSEGUIR UBICACION DEL DISPOSITIVO
        Input.location.Start();
        Latitud = Input.location.lastData.latitude;
        Contador++;
        Longitud= Input.location.lastData.longitude;
        //string texto = Latitud.ToString() + "-" + Longitud;
        //Debug.Log(texto);
        //this.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = texto;
        //----------------------------------------------------------------
        //ENVIO DE LA INFORMACION
        StartCoroutine(CorrutinaSubida("Latitud", Contador.ToString()));
        StartCoroutine(CorrutinaSubida("Longitud", Contador.ToString()));
        Contador++;
    }
    public void RegresoMenuPrincipal(){
  //    SceneManager.LoadScene("MenuPrincipal");
        //Application.Quit();
  //    Debug.Log("Se pulso el boton");
        Debug.Log(Actualizacion);
    }
}