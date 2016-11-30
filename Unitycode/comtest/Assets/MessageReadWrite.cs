using UnityEngine;
using System.Collections;

public class MessageReadWrite : MonoBehaviour {

    public SerialController serialController;
    private int randomizer;
    public int maxRange;

    void Start()
    {
        serialController = GameObject.Find("SerialController").GetComponent<SerialController>();
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

        // Check if the message is plain data or a connect/disconnect event.
        if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_CONNECTED))
            Debug.Log("Connection established");
        else if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_DISCONNECTED))
            Debug.Log("Connection attempt failed or disconnection detected");
        else
            Debug.Log("Message arrived: " + message);
    }
}
