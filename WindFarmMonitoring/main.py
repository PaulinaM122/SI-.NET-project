import time
import random
import paho.mqtt.client as mqtt
import json
from datetime import datetime


def generate_data(sensor_type):
    if sensor_type == "temperature":
        return round(random.uniform(-10, 40), 2)
    elif sensor_type == "wind_speed":
        return round(random.uniform(0, 25), 2)
    elif sensor_type == "wind_direction":
        return random.choice([1, 2, 3, 4]) #["N", "S", "E", "W"]
    elif sensor_type == "rpm":
        return random.randint(0, 3000)


def on_connect(client, userdata, flags, reasonCode, properties):
    print(f"Połączono z brokerem MQTT. Kod powrotu: {reasonCode}")


def on_publish(client, userdata, mid):
    print(f"Wiadomość wysłana, ID: {mid}")


def main():
    client = mqtt.Client("SensorSimulator", protocol=mqtt.MQTTv5)

    # Przypisanie callbacków
    client.on_connect = on_connect
    client.on_publish = on_publish

    # Połączenie z brokerem MQTT
    client.connect("localhost", 1883, 60)

    client.loop_start()

    sensors = [
        {"sensorId": i, "type": random.choice(["temperature", "wind_speed", "wind_direction", "rpm"])}
        for i in range(1, 17)
    ]

    while True:
        sensor = random.choice(sensors)
        value = generate_data(sensor["type"])

        topic = f"windfarm/sensors/{sensor['type']}"
        message = {
            "SensorId": sensor["sensorId"],
            "SensorType": sensor["type"],
            "Value": value,
            "Timestamp": datetime.utcnow().isoformat()
        }
 
        client.publish(topic, json.dumps(message))
        print(f"Wysłano: {message} do tematu: {topic}")

        # Losowy czas między wysyłkami (np. od 1 do 5 sekund)
        time.sleep(random.uniform(1, 5))


if __name__ == "__main__":
    main()
