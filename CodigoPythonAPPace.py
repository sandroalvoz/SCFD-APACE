import threading
import select
import socket
from threading import Thread
import requests
import errno
import time
import requests
from firebase import firebase				#pip install firebase
from paho.mqtt import client as mqtt_client #pip install paho-mqtt
import paho.mqtt
import firebase_admin						#pip install firebase_admin
from firebase_admin import credentials,messaging
import random
import paho.mqtt.subscribe as subscribe

semaforoPublicacion=threading.Semaphore(1)

cred = credentials.Certificate("appace-39c51-firebase-adminsdk-5x9yv-387365e56d.json")#.json
firebase_admin.initialize_app(cred)

#SERVIDOR TCP, el programa de python actuara como servidor TCP, escuchando en el puerto 2705
IP = "0.0.0.0"
puerto = 2705
socket1 = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
#Se define socket1 como un objeto de tipo puerto, concretamente un puerto TCP
socket1.bind((IP, puerto))
#Se fija o se "vincula" socket1 al socket 0.0.0.0:2705

#BROKER MQTT
broker='192.168.1.101' #IP del broker MQTT
port=3015
topic="zona"
client_id=f'python-mqtt-{random.randint(0,1000)}'
#client=Client(client_id=client_id, clean_session=True,userdata=None,protocol=mqtt_client.MQTTv311,transport="tcp")
client=mqtt_client.Client(client_id=client_id, clean_session=True,userdata=None,protocol=mqtt_client.MQTTv311,transport="tcp")
print(client)
client.connect('192.168.1.101', port=3015, keepalive=60, bind_address="")

def lecturaMQTTBotonPiscina(): #Thread que leera el estado del ESP32 de la piscina.
	while True:
		msg=subscribe.simple("alarma",qos=2,hostname='192.168.1.101',port=3015)
		print(msg.payload)
		informacionTopico=(str(msg.payload)[1::]).replace("'","")
		if informacionTopico=="Boton pulsado": #Si el boton esta pulsado, se envia una llamada de socorro.
			print("El boton esta pulsado")	
			with semaforoPublicacion:
				reporte("piscina") #Se atualizan los topicos MQTT y se envia una notificacion a la aplicacion movil
		elif informacionTopico=="Boton no pulsado":
			print("El boton no esta pulsado")
		elif informacionTopico=="AlarmaApagada": #Corresponde al reinicio de la alarma.
			with semaforoPublicacion:
				client.publish("zona",payload="AlarmaApagada",qos=2,retain=True) 
				#No es necesario enviar una notificacion a la aplicacion movil, por lo que solo se actualizaran los topicos MQTT

def sendPush(title, msg, registration_token, dataObject=None): #Funcion para enviar notificacion push a la aplicacion movil
	message = messaging.MulticastMessage(notification=messaging.Notification(title=title,body=msg),data=dataObject,tokens=registration_token,)
	# SE DEFINE LA NOTIFICACION PUSH
	response = messaging.send_multicast(message) #SE ENVIA LA NOTIFICACION PUSH
	print('Successfully sent message:', response)

def reporte(zona): #Funcion para actualizar topicos MQTT y enviar notificaciones push a la aplicacion movil
	client.publish("zona",payload=zona,qos=2,retain=True)		#PUBLICACION TOPICO MQTT	
	title=f"Emergencia en {zona}"								#ENVIO INFORMACION A FIREBASE
	msg=""
	token  = ["",""]											#El token se mantiene fijo. No lo puedo difundir asi que no lo incluyo aqui
	sendPush(title,msg,token,)									#Se envia la notificacion push

def conexion(conn,addr):
	flujo=1
	while flujo==1:
		data=conn.recv(1024) #2048
		if not data: break
		else:	#POR CONDICIONES DE QUIEBRE, EL 'ELSE:' ES INNECESARIO (TODO LO QUE INCLUYE SE PODRIA ESCRIBIR SIN INDENTACION),
				#PERO LO DEJO POR SI ACASO YA QUE NUNCA SE HA CUMPLIDO LA CONDICION DE QUIEBRE Y NO HE PODIDO ASEGURARME DE ELLO
			info=str(data) #PARSEO INFORMACION HTTP
			#AQUI DEBERIA INCLUIR UN FILTRO PARA SOLO INTERPRETAR PETICIONES HTTP. 
			#YA QUE SI RECIBO ALGO QUE NO SEA UNA PETICION HTTP, SE GENERARIA UNA EXCEPCION
			claves=(info[info.find('{')::]).split(',')
			lat=float(claves[0].split(':')[1])
			long=float((claves[1].split(':')[1]).replace("'","").replace("}",""))
			print(f"La latitud es:	{lat}\n\rLa longitud es:	{long}")
			zona=1
			#ACTUALIZACION TOPICOS BROKER MQTT
			with semaforoPublicacion: #Se hace referencia al semaforo 'semaforoPublicacion'
				reporte(zona)
			flujo=0

t=Thread(target=lecturaMQTTBotonPiscina) 
t.start() #Se inicia el thread que lee el estado del ESP32 de la piscina

while True: #Bucle que gestiona las peticiones HTTP que recibe el servidor Python
	socket1.listen(10) #Escucha en el socket socket1 (0.0.0.0:2705)
	ready, _, _ = select.select([socket1], [], [], 1)
	#Estructura select. Cuando se conecte un cliente al socket --> ready=True
	if ready:
		try:
			conn, addr = socket1.accept() #Se gestiona la conexion del cliente recien conectado
			t = Thread(target=conexion, args=[conn,addr]) #y se crea un Thread con su informacion
			t.start() #este thread gestionara la conexion con el cliente
		except socket.error as e: #Registro de excepciones.
			if e.errno != errno.EWOULDBLOCK:
				print(f"Error accepting client connection: {e}")
			time.sleep(0.1)