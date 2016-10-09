using UnityEngine;
using System.Collections;
using System.IO;

public class ioWriter : MonoBehaviour {



    StreamReader sReader;
    StreamWriter sWriter;
    // Use this for initialization
    void Start () {

        try{ 
        sWriter = File.CreateText("\\Assets\\userData\\test.txt");
        sWriter.WriteLine("profielnummer");
        sWriter.WriteLine("naam");
        sWriter.WriteLine("niveau");
        sWriter.WriteLine("snelheid");
        }
        catch
        {

        }
        finally {
            sWriter.Close();
        }

        try
        {
            sReader = File.OpenText("test.txt");
            string userData = sReader.ReadLine();
            while (userData != null)
            {
                Debug.Log(userData);
                userData = sReader.ReadLine();
            }
           

        }
        catch
        {

        }
        finally
        {
            sReader.Close();
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
