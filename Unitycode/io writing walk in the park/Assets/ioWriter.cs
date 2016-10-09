using UnityEngine;
using System.Collections;
using System.IO;

public class ioWriter : MonoBehaviour
{

    profile testProfile;
    string[] dataSplitted;
    StreamReader sReader;
    StreamWriter sWriter;
    profile newProfile;
    // Use this for initialization
    void Start()
    {
        readPatientData();
        writePatientData();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void readPatientData()
    {
        try
        {
            sReader = File.OpenText("profiles.txt");
            string userData = sReader.ReadLine();
            while (userData != null)
            {
                dataSplitted = userData.Split(';');
                Debug.Log(dataSplitted[1]);
                newProfile = new profile(dataSplitted[0], dataSplitted[1], dataSplitted[2]);
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
    private void writePatientData()
    {
        try
        {
            sWriter = File.CreateText("profilesTest1.txt");
            sWriter.WriteLine(newProfile.writePatientData());
        }
        catch
        {

        }
        finally
        {
            sWriter.Close();
        }
    }
}
public class profile
{
    string userNumber;
    string name;
    string skill;
    public profile() { }

    public profile(string tUserNumber, string tName, string tSkill)
    {
        userNumber = tUserNumber;
        name = tName;
        skill = tSkill;
    }
    public string UserNumber
    {
        get { return userNumber; }
    }
    public string UserName
    {
        get { return name; }
    }
    public string UserSkill
    {
        set { skill = value; }
        get { return skill; }
    }
    public string writePatientData()
    {
        return name + ';' + userNumber + ';' + skill;
    }
}
