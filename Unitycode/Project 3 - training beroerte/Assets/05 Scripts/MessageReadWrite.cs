using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageReadWrite : MonoBehaviour {

    public SerialController serialController;
    private int randomizer;
    public int maxRange;
    public int maxSpeed = 12;
    public double minSpeed = 0.2;
    private List<double> speedList;
    private double messageDouble;
    public double calculatedSpeed;
    private double median;

    void Start()
    {
        serialController = GameObject.Find("SerialController").GetComponent<SerialController>();
        speedList = new List<double>();
    }

    void Update()
    {
        randomizer = Random.Range(0, maxRange);
        //send data
        if (randomizer == 6)
        {
            Debug.Log("Sending A");
            serialController.SendSerialMessage("A");
            }

        //recieve data
        string message = serialController.ReadSerialMessage();
        if (message == null)
            return;

        Debug.Log("data recieved: " + message);
        messageDouble = double.Parse(message);
        if (messageDouble > minSpeed && messageDouble < maxSpeed) //if value is between the min and max values
        {
            speedList.Add(messageDouble);
        }
        if (speedList.Count == 10)  //calculate median of last 10 values
        {
            CalculateSpeed();
            speedList.Clear();
        }



        // Check if the message is plain data or a connect/disconnect event.
        if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_CONNECTED))
            Debug.Log("Connection established");
        else if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_DISCONNECTED))
            Debug.Log("Connection attempt failed or disconnection detected");
        else
            Debug.Log("Message arrived: " + message);
    }
    void CalculateSpeed()
    {
        // speedList.Sort();
        //calculatedSpeed = speedList[3];
        calculatedSpeed = 0;

        foreach (double speedData in speedList)
        {
            median += speedData;
        }
        calculatedSpeed = (median / 10);
        Debug.Log("speed is: " + calculatedSpeed);
    }

}
