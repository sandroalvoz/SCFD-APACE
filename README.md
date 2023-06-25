# SCFD-APACE
Repositorio del proyecto "Sistema centralizado y funciones domóticas a instalar en APACE", realizado por Sandro Alvoz y Luis Gómez González

Este proyecto consiste en un sistema centralizado con un sistema de llamadas de socorro como su primera función. Este sistema de llamada de socorro consta de dos partes: el cliente y el servidor.

El cliente puede interactuar con el servidor de dos maneras: a través de una aplicación móvil, y a través de un micromódulo ESP32. El servidor procesará la información enviada por el cliente y se comunicará con él
cuando sea necesario, activando una alarma en un micromódulo ESP32 y enviando una notificación a la aplicación móvil.

Inicialmente, se utilizó Unity para desarrollar la aplicación móvil; y Node-Red, junto a un script en PHP, para procesar las distintas llamadas de socorro (incluido en la carpeta "1.0"). Sin embargo, hubo problemas a la hora de implementar las
notificaciones en la aplicación móvil, y mi trabajo con sockets me mostró una mejor alternativa a la hora de procesar las llamadas de socorro, por lo que acabamos usando React Native para desarrollar la aplicación
móvil y Python para procesar la llamadas de socorro (incluido en la carpeta "2.0"). La programación del micromódulo ESP32 permanece sin cambios mayores.
