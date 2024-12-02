import time
import random
import paho.mqtt.client as mqtt
import json
from datetime import datetime

def generate_data():
    sensors = ["temperature", "wind_speed", "wind_direction", "rpm"]
    values = {
        "temperature": round(random.uniform(-10, 40), 2),
        "wind_speed": round(random.uniform(0, 25), 2),
        "wind_direction": random.choice(["N", "S", "E", "W"]),
        "rpm": random.randint(0, 3000),
    }
    selected_sensor = random.choice(sensors)
    return selected_sensor, values[selected_sensor]

def on_connect(client, userdata, flags, reasonCode, properties):
    print(f"Połączono z brokerem MQTT. Kod powrotu: {reasonCode}")

def on_publish(client, userdata, mid):
    print(f"Wiadomość wysłana, ID: {mid}")

def main():
    # Utworzenie klienta MQTT
    client = mqtt.Client("SensorSimulator", protocol=mqtt.MQTTv5)  # Określenie wersji MQTT v5

    # Przypisanie callbacków
    client.on_connect = on_connect
    client.on_publish = on_publish

    # Połączenie z brokerem MQTT
    client.connect("localhost", 1883, 60)

    # Rozpoczęcie pętli komunikacji z brokerem
    client.loop_start()

    while True:
        sensor, value = generate_data()
        topic = f"windfarm/sensors/{sensor}"
        message = {
            "SensorType": sensor,
            "Value": value,
            "Timestamp": datetime.utcnow().isoformat()
        }
        client.publish(topic, json.dumps(message))
        print(f"Wysłano: {message} do tematu: {topic}")
        time.sleep(2)

if __name__ == "__main__":
    main()
