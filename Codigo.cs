//CODIGO DEL MENU PRINCIPAL DINAMICO
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
//using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
public class Codigo : MonoBehaviour
{
    //plantilla Botones Menu. Con esto, se podrán crear botones para acceder a cada una de las aplicaciones
    public UnityEngine.UI.Button Boton;
    public GameObject LayoutBotones; //se define Boton como GameObject. Desde Unity, se le importa el prefab de un botón
    public string[] TextoBotones; //se define la variable TextoBotones como un vector
    void Start()
    {
        int numerofunciones = 4; //Definicion de numerofunciones, variable en la que se almacenará el numero de funciones a implementar en el menu de la aplicacion
        //definicion del array TextoBotones, donde se almacenaran los nombres de cada funcion a la que accederemos desde el menu
        TextoBotones = new string[numerofunciones];
        TextoBotones[0] = "Hola";
        TextoBotones[1] = "esto";
        TextoBotones[2] = "es un texto";
        TextoBotones[3] = "para el boton";
        //RectTransform RT=LayoutBotones.GetComponent(typeof (RectTransform)) as RectTransform;
        //------------------------------------------------------------------------------------
        //aumento del tamaño del scroll vertical, EO(BotonesScrollVertical), por cada objeto
        RectTransform RT = LayoutBotones.GetComponent<RectTransform>();
        RT.sizeDelta = new Vector2(1000, 400 + (200 * numerofunciones));
        for (int i = 0; i <= TextoBotones.Length - 1; i++)
        {
            UnityEngine.UI.Button Boton1 = Instantiate(Boton, LayoutBotones.transform); //se genera un nuevo Boton, contenido como hijo del objeto LayoutBotones.
            //Boton y LayoutBotones son prefabs, plantillas de objetos ya existentes en nuestro editor gráfico de Unity
            Boton1.GetComponentInChildren<TextMeshProUGUI>().text = TextoBotones[i];
            //se modifica la componente de texto del botón que hemos creado anteriormente, almacenando en ella uno de los valores
            //contenidos en el array TextoBotones
            //------------------------------------------------------------------------------------
            //Boton1.GetComponent<Button>().onClick.AddListener(delegate { });
            Boton1.onClick.AddListener(delegate { Cambio("OtraEscena"); });
            //vincular cada objeto a una ventana o escena
        }
    }
    void Update()
    {
    }
    void Cambio(string Escena){
        SceneManager.LoadScene(Escena);
    }
    public void CambioEscena(string Escena) {
        SceneManager.LoadScene(Escena);
    }
}
