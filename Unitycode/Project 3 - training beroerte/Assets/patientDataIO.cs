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
    List<string> patientsToSort;
    List<string> patientsToAdd;
    

    //ui public
    public Dropdown possiblePatientSelect;
    public Dropdown patientSearch;
    public InputField inputSearch;
    public Text txtProfileDetails;

    // Use this for initialization
    void Start()
    {
        Patients = new List<profile>();
        patientsToSort = new List<string>();
        patientsToAdd = new List<string>();
        readPatientData();
        showPatientData();


       // writePatientData();

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

    public void showPatientDetails()
    {
        foreach (profile patientDetailsToShow in Patients)
        {
            if (patientDetailsToShow.patientSelectCheck(patientsToAdd[possiblePatientSelect.value]))
            {
                txtProfileDetails.text = patientDetailsToShow.showPatientDetails();
            }
        }
       // txtProfileDetails.text = Patients patientsToAdd[possiblePatientSelect.value];
    }

    public void showPatientData()
    {
        inputSearch.text = inputSearch.text.ToLower();
        patientsToSort.Clear();
        patientsToAdd.Clear();
        possiblePatientSelect.ClearOptions();
        switch (patientSearch.value)
        {
            case 0:
                foreach (profile patient in Patients)
                {
                    patientsToSort.Add(patient.UserName);
                }
                break;
            case 1:
                foreach (profile patient in Patients)
                {
                    patientsToSort.Add(patient.UserNumber);
                }
                break;

            case 2:
                foreach (profile patient in Patients)
                {
                    //   patientsToSort.Add(patient.UserSkill + " " + patient.UserName);
                    patientsToSort.Add(patient.UserName);
                }
                break;
        }
        foreach (string patientDataToSort in patientsToSort)
        {
            /* for (int i = 0; i < patientDataToSort.Length; i++)
             {
            if( ! (patientDataToSort[i] == inputSearch.text[i]))
                 {

                 }

             }*/
            if (recursivePatientData(0, patientDataToSort))
            {
                patientsToAdd.Add(patientDataToSort);
            }
            
        }
        possiblePatientSelect.AddOptions(patientsToAdd);
        showPatientDetails();
    }
    private bool recursivePatientData(int i, string patientDataToSort)
    {
        patientDataToSort = patientDataToSort.ToLower();
        if (i == inputSearch.text.Length)
        {
            return true;
        }
        else
        {
            if (patientDataToSort[i] == inputSearch.text[i])
            {
                i++;
                recursivePatientData(i, patientDataToSort);
                return true;
            }
            else
            {
                return false;
            }
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


   // methods
    public string writePatientData()
    {
        return userNumber + ';' + name + ';' + skill;
    }
    public bool patientSelectCheck(string nameToTest)
    {
        if (nameToTest == name || nameToTest == userNumber)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public string showPatientDetails()
    {
        
      return "Patiëntnummer: " + userNumber + "\nPatiëntnaam: " + name + "\nNiveau: " + skill;
    }
    
}
