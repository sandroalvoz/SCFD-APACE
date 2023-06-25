import React, { useEffect, useState } from 'react';
import { View, StyleSheet, TouchableOpacity, Text, SafeAreaView, ScrollView, StatusBar, PushNotificationIOS} from 'react-native';
import Geolocation from '@react-native-community/geolocation';
import messaging from '@react-native-firebase/messaging';

interface NavigationProps {
  navigate: (screen: string) => void;
}

const Screen1: React.FC<NavigationProps> = ({ navigate }) => {
  const [location, setLocation] = useState<{ latitude: number; longitude: number } | null>(null);

  const handleButtonPress = async () => {
    Geolocation.getCurrentPosition(
      position => {
        const { latitude, longitude } = position.coords;
        setLocation({ latitude, longitude });
        sendRequest(latitude, longitude);
      },
      error => {
        console.log('Error getting location:', error);
      }
    );
  };

  const sendRequest = (latitude: number, longitude: number) => {
    const requestData = { latitude, longitude };

    fetch('http://192.168.1.61:2705', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(requestData),
    })
      .then(response => response.json())
      .then(data => {
        console.log('Response:', data);
      })
      .catch(error => {
        console.log('Error sending request:', error);
      });
  };

  const navigateToScreen2 = () => {
    navigate('Screen2');
  };

  return (
    <View style={styles.container}>
      <View style={styles.buttonContainer}>
        <TouchableOpacity onPress={handleButtonPress} style={styles.button}>
          <Text style={styles.buttonText}>Enviar Llamada de Auxilio</Text>
        </TouchableOpacity>
      </View>
    </View>
  );
};

const Screen2: React.FC = () => {
  return (
    <View style={styles.container}>
      <Text style={styles.screenText}>Segunda pantalla</Text>
    </View>
  );
};

const App: React.FC = () => {
  const checkToken = async() => {
    const fcmToken = await messaging().getToken();
    if (fcmToken){
      console.log("El Token es: ", fcmToken);
    }
  }
  checkToken();

  useEffect(()=> {
    /*const foregroundSubscriber=messaging().onMessage(
      async (remoteMessage) => {
        console.log("Se ha recibido la notificacion push", remoteMessage);
        remoteMessage.notification;
      },
    );*/
    //const backgroundSubscriber= messaging().setBackgroundMessageHandler(
      //async (remoteMessage) => {
        //console.log("Se ha recibido la notificacion push en background", remoteMessage)
      //},
    //);
    messaging().onMessage(
      async (remoteMessage) => {
        console.log("Se ha recibido la notificacion push", remoteMessage);
        remoteMessage.notification;
      },
    );
    messaging().setBackgroundMessageHandler(
      async (remoteMessage) => {
        console.log("Se ha recibido la notificacion push en background", remoteMessage);
        /*const fcmToken=await messaging().getToken();
        if (fcmToken){
          console.log("El Token es: ", fcmToken);
        }*/
        remoteMessage.notification;
      },
    );

    /*return ()=> {
      foregroundSubscriber();
    };*/
  },[]);
  
  const [currentScreen, setCurrentScreen] = useState<string>('Screen1');

  const navigateToScreen = (screen: string) => {
    setCurrentScreen(screen);
  };

  const renderScreen = () => {
    switch (currentScreen) {
      case 'Screen1':
        return <Screen1 navigate={navigateToScreen} />;
      case 'Screen2':
        return <Screen2 />;
      default:
        return null;
    }
  };

  return (
    <View style={styles.container}>
      {renderScreen()}
      <View style={styles.bottomMenu}>
        <TouchableOpacity onPress={() => navigateToScreen('Screen1')} style={styles.menuItem}>
          <Text style={styles.menuItemText}>Screen 1</Text>
        </TouchableOpacity>
        <TouchableOpacity onPress={() => navigateToScreen('Screen2')} style={styles.menuItem}>
          <Text style={styles.menuItemText}>Screen 2</Text>
        </TouchableOpacity>
      </View>
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  bottomMenu: {
    flexDirection: 'row',
    justifyContent: 'space-around',
    alignItems: 'center',
    backgroundColor: '#ccc',
    height: 60,
  },
  menuItem: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  menuItemText: {
    fontSize: 16,
    fontWeight: 'bold',
  },
  button: {
    backgroundColor: 'orange',
    padding: 40,
    borderRadius: 5,
    marginBottom: 10,
  },
  buttonText: {
    color: 'black',
    fontSize: 16,
    fontWeight: 'bold',
  },
  menuButton: {
    backgroundColor: 'gray',
    padding: 10,
    borderRadius: 5,
  },
  menuButtonText: {
    color: 'white',
    fontSize: 16,
    fontWeight: 'bold',
  },
  screenText: {
    fontSize: 24,
    fontWeight: 'bold',
    marginTop: 20,
  },
  buttonContainer: {
    width: '90%',
    justifyContent: 'center',
  },
});

export default App;
