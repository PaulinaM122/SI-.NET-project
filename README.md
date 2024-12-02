# WindFarm Monitoring System

This project provides a monitoring system for wind farms. It collects sensor data using an MQTT broker and stores the information in a MongoDB database. The backend is built using .NET Core and is designed to provide endpoints for querying the sensor data.

---

## Features
- **MQTT Integration**: Subscribes to an MQTT broker to receive real-time sensor data.
- **MongoDB Storage**: Stores sensor data for retrieval and analysis.
- **REST API**: Exposes endpoints for querying the data.

---

## Prerequisites

1. **Install the following tools**:
   - [.NET SDK](https://dotnet.microsoft.com/download) (version 6 or higher)
   - [MongoDB](https://www.mongodb.com/try/download/community)
   - [Mosquitto MQTT Broker](https://mosquitto.org/download/)
   - [Python](https://www.python.org/downloads/) (if you want to run the sensor simulator script)

2. **Configure your environment**:
   - Ensure MongoDB is running locally on port `27017`.
   - Ensure Mosquitto MQTT Broker is running locally on port `1883`.

---

## Setup Instructions

### **Step 1: Clone the Repository**
```bash
git clone https://github.com/PaulinaM122/SI-.NET-project.git
cd windfarm-monitoring
```

### **Step 2: Configure the Backend**

1. **Update `appsettings.json`**:
   Ensure the following configuration in `WindFarmMonitoring/appsettings.json` matches your MongoDB setup:
   ```json
   {
     "MongoDB": {
       "ConnectionString": "mongodb://localhost:27017",
       "DatabaseName": "WindFarm"
     }
   }
   ```

2. **Restore Dependencies**:
   ```bash
   dotnet restore
   ```

3. **Run the Backend**:
   ```bash
   dotnet run
   ```
   The API will be available at `http://localhost:5016`.

---

### **Step 3: Run the Sensor Simulator**

1. Navigate to the Python simulator directory:
   ```bash
   cd python_simulator
   ```

2. Install dependencies:
   ```bash
   pip install -r requirements.txt
   ```

3. Run the script:
   ```bash
   python main.py
   ```
   This will simulate sensor data and publish it to the MQTT broker.

---

## API Endpoints

### **GET /api/SensorData**
Retrieve all sensor data.

#### **Example Request**:
```bash
curl -X GET http://localhost:5016/api/SensorData
```

### **GET /api/sensors/data/{type}**
Retrieve sensor data of a specific type (e.g., temperature, wind speed).

#### **Example Request**:
```bash
curl -X GET http://localhost:5016api/SensorData/temperature
```

---

## Troubleshooting

### Common Issues:

1. **Cannot Connect to MongoDB**:
   - Ensure MongoDB is running on the expected port (27017).
   - Verify the connection string in `appsettings.json`.

2. **Cannot Connect to MQTT Broker**:
   - Verify that Mosquitto is running on port 1883.
   - Check if the simulator script is correctly publishing messages.

3. **No Data in MongoDB**:
   - Ensure the Python simulator is running and publishing data.
   - Check the application logs for errors.

---

## Contributors
- **Joanna Symczyk** - Developer
- **Paulina Machcińska** - Developer

---

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

