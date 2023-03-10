//codigo original de: https://github.com/ioticos/esp32_ioticos_broker_hello_world
#include <Arduino.h>
#include <WiFi.h>
#include <PubSubClient.h>
//**************************************
//*********** MQTT CONFIG **************
//**************************************
const char *mqtt_server = "192.168.1.101"; //IP del broker MQTT
const int mqtt_port = 3015; //puerto utilizado
const char *mqtt_user = "esp32";//nombre de usuario y contraseña
const char *mqtt_pass = "Marina2022";
const char *root_topic_subscribe = "estado";//topico suscripcion raiz
const char *root_topic_publish = "alarma";//topico publicacion raiz
//**************************************
//*********** WIFICONFIG ***************
//**************************************
const char* ssid = "SSID"; //SSID y contraseña de la WLAN a la que se conecta el ESP32, que resulta ser la de mi casa. No me robéis ancho de banda.
const char* password =  "contraseña";
//**************************************
//*********** GLOBALES   ***************
//**************************************
WiFiClient espClient;
PubSubClient client(espClient);
char msg[25];
long count=0;
//************************
//** F U N C I O N E S ***
//************************
void callback(char* topic, byte* payload, unsigned int length);
void reconnect();
void setup_wifi();
void setup() {
  pinMode(15,INPUT);
  Serial.begin(115200);
  setup_wifi();
  client.setServer(mqtt_server, mqtt_port);
  client.setCallback(callback);
}
void loop() {
  if (!client.connected()) {
    reconnect(); //si el cliente no está conectado, se ejecuta la función reconnect()
  }
  Serial.print("La lectura es: ");
  Serial.println(digitalRead(15));//lectura del pin digital 15, conectado a un botón
  if(client.connected()){
  if (digitalRead(15)==0){
    //MQTT no puede enviar directamente la señal leída en el pin 15, por lo que se modificará
    //el contenido enviado a través de una estructura if()
      client.publish(root_topic_publish, "Boton no pulsado");
    } else {
      client.publish(root_topic_publish, "Boton pulsado");
      }
      delay(300);}
  client.loop(); //se incluye por el funcionamiento de PubSubClient
}
//*****************************
//***    CONEXION WIFI      ***
//*****************************
void setup_wifi(){
  delay(10);
  // Se conecta a la red Wi-Fi
  Serial.println();
  Serial.print("Conectando a ssid: ");
  Serial.println(ssid);
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println("");
  Serial.println("Conectado a red WiFi!");
  Serial.println("Dirección IP: ");
  Serial.println(WiFi.localIP());
}
//*****************************
//***    CONEXION MQTT      ***
//*****************************
void reconnect() {
  while (!client.connected()) {
    Serial.print("Intentando conexión Mqtt...");
    // Creamos un cliente ID
    String clientId = "InstalacionAPACE"; //"IOTICOS_H_W_"
    // Se crea un clientId aleatorio para evitar fallos al colisionar con otros dispositivos
    clientId += String(random(0xffff), HEX);
    // Se intenta conectar al broker
    if (client.connect(clientId.c_str(),mqtt_user,mqtt_pass)) {
      Serial.println("Conectado!");
      // Nos suscribimos
      if(client.subscribe(root_topic_subscribe)){
        Serial.println("Suscripcion ok");
        // La suscripcion se ha realizado correctamente
      }else{
        Serial.println("fallo Suscripción");
        // La suscripcion ha fallado.
      }
    } else {
      Serial.print("falló con error -> ");
      Serial.print(client.state());
      Serial.println(" Intentamos de nuevo en 5 segundos");
      delay(5000);
    }
  }
}
//*****************************
//***       CALLBACK        ***
//*****************************
void callback(char* topic, byte* payload, unsigned int length){
  String incoming = "";
  Serial.print("Mensaje recibido desde -> ");
  Serial.print(topic);
  Serial.println("");
  for (int i = 0; i < length; i++) {
    incoming += (char)payload[i];
    //los caracteres de payload, la informacion recibida, se traspasan uno a uno a la variable incoming
  }
  incoming.trim();
  // Se elimina el contenido adicional de incoming
  Serial.println("Mensaje -> " + incoming);
}
