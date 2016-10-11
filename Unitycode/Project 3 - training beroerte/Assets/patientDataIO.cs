using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
public class patientDataIO : MonoBehaviour {
    profile testProfile;
    string[] dataSplitted;
    StreamReader sReader;
    StreamWriter sWriter;
    profile newProfile;
    List<profile> Patients;
    Dropdown possiblePatientSelect;
    // Use this for initialization
    void Start()
    {
        Patients = new List<profile>();
        readPatientData();
        writePatientData();
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    void btnPatientAddClicked()
    {

    }
    void btnPatientSelectClicked()
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
                Debug.Log("test");
                dataSplitted = userData.Split(';');
                Patients.Add(new profile(dataSplitted[0], dataSplitted[1], dataSplitted[2]));
                userData = sReader.ReadLine();

            }

            Debug.Log(Patients.Count);
            Debug.Log(Patients[0]);
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
            sWriter = File.CreateText("profiles.txt");
            //     sWriter.WriteLine(newProfile.writePatientData());
            foreach (profile patientData in Patients)
            {
                sWriter.WriteLine(patientData.writePatientData());
            }
                      //sWriter.WriteLine("123456;Laure Leirs;niveau 3");
                    // sWriter.WriteLine("123;Christophe Claessens;Niveau2");
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
        return userNumber + ';' + name + ';' + skill;
    }
}
