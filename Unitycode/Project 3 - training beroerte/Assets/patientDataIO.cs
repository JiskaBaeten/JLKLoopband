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
    bool patientFound;
    

    //ui public
    public Dropdown possiblePatientSelect;
    public Dropdown patientSearch;
    public InputField inputSearch;
    public Text txtProfileDetails;
    public GameObject addPatientInterface;
    public GameObject patientViewInterface;

    // Use this for initialization
    void Start()
    {
        addPatientInterface.SetActive( false);
        patientViewInterface.SetActive(true);
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


    public void btnPatientAddClicked()
    {
        patientViewInterface.SetActive(false);
        addPatientInterface.SetActive(true);
    }
    
    public void btnPatientEditClicked()
    {
        btnPatientAddClicked();
        foreach (profile patientDetailsToShow in Patients)
        {
            if (patientDetailsToShow.patientSelectCheck(patientsToAdd[possiblePatientSelect.value]))
            {
               //change txtboxes to value
            }
        }
    }

    public void btnPatientDeleteClicked()
    {

    }

    public void btnConfirmPatientAddClicked()
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
                Patients.Add(new profile(dataSplitted[0], dataSplitted[1], dataSplitted[2], dataSplitted[3], dataSplitted[4]));
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

    public void showPatientDetails()
    {
      
        foreach (profile patientDetailsToShow in Patients)
        {
            if (patientDetailsToShow.patientSelectCheck(patientsToAdd[possiblePatientSelect.value]))
            {
                txtProfileDetails.text = patientDetailsToShow.showPatientDetails();
            }
        }
    }

    public void showPatientData()
    {
        patientFound = false;
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
                    patientsToSort.Add(patient.UserBirthday);
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
                patientFound = true;
            }
            
        }
        if (!patientFound)
        {
            patientsToAdd.Add("Niemand gevonden");
        }
        possiblePatientSelect.AddOptions(patientsToAdd);
        showPatientDetails();
    }

    private bool recursivePatientData(int i, string patientDataToSort)
    {
        patientDataToSort = patientDataToSort.ToLower();
        if (i == inputSearch.text.Length || i == patientDataToSort.Length)
        {
            Debug.Log(inputSearch.text + patientDataToSort);
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
    string birthday;
    string extraInfo;
    public profile() { }

    public profile(string tUserNumber, string tName, string tSkill, string tBirthday, string tExtraInfo)
    {
        userNumber = tUserNumber;
        name = tName;
        skill = tSkill;
        birthday = tBirthday;
        extraInfo = tExtraInfo;
    }
    public string UserNumber
    {
        set { UserNumber = value; }
        get { return userNumber; }
    }
    public string UserName
    {
        set { name = value; }
        get { return name; }
    }
    public string UserSkill
    {
        set { skill = value; }
        get { return skill; }
    }
    public string UserBirthday
    {
        set { birthday = value; }
        get { return birthday; }
    }
    public string UserExtraInfo
    {
        set { extraInfo = value; }
        get { return extraInfo; }
    }


   // methods
    public string writePatientData()
    {
        return userNumber + ';' + name + ';' + skill + ";" + birthday + ";" + extraInfo;
    }
    public bool patientSelectCheck(string nameToTest)
    {
        if (nameToTest == name || nameToTest == userNumber || nameToTest == birthday)
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
        
      return "Patiëntnummer: " + userNumber + "\nPatiëntnaam: " + name + "\nNiveau: niveau " + skill + "\nGeboortedatum: " + birthday + "\nextra info: " + extraInfo;
    }
    
}
