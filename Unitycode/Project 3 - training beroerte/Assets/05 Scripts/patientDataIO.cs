﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using Environment = System.Environment;
using UnityEngine.SceneManagement;
public class patientDataIO : MonoBehaviour
{
    //to read and write patient data
    string[] dataSplitted;
    StreamReader sReader;
    StreamWriter sWriter;
    string readWritePath;

    //to manipulate patient data
    List<profile> Patients; //overall patient list
    List<profile> patientsToChange;
    List<string> patientsToSort;
    List<string> patientsToAdd;
    profile patientSelected; //player with wich the game will continue

    //multiple methods
    bool patientFound; //if any patient matches the search terms
    bool editScreen; //check if the player was added from edit of add (if edit, delete old data)
    int patientCount; //number of patients in system for patient number
    string patientCountToString;


    //ui public
    public Text txtProfileDetails;
    public GameObject addPatientInterface;
    public GameObject patientViewInterface;
    public GameObject chooseSceneInterface;

    public InputField inputName;
    public InputField inputBirthday;
    public InputField inputNumber;
    public InputField inputExtra;
    public InputField inputLevel;
    public InputField inputGender;
    public Text txtError;
    public Button btnConfirmData;
    public Button btnCancelData;

    public Button btnChooseHome;
    public Button btnChoosePark;
    public Button btnCancelChoose;

    //view interface
    public Button btnEditPatient;
    public Button btnSelectPatient;
    public Button btnDeletePatient;
    public Button btnAddPatient;
    public Dropdown possiblePatientSelect;
    public Dropdown patientSearch;
    public InputField inputSearch;

    //ui private
    InputField[] allInputs;
    Button[] interfaceButtonsView;
    Button[] interfaceButtonsChoose;
    byte addPatientSelectedInput;
    byte viewPatientSelectedInput;
    byte chooseProjectSelectedInput;

    int currentSelectedDropdownValuePatientdata;
    int currentSelectedDropdownValuePatientSearch;

    void Start()
    {
        Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\A walk in the park");
        readWritePath = Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        readWritePath += "\\A walk in the park\\PatientProfiles.txt";
        editScreen = false;
        showStartScreen();
        Patients = new List<profile>();
        patientsToChange = new List<profile>();
        patientsToSort = new List<string>();
        patientsToAdd = new List<string>();
        readPatientData();
        showPatientData();



        addPatientSelectedInput = 0;
        viewPatientSelectedInput = 4;
        chooseProjectSelectedInput = 0;

        currentSelectedDropdownValuePatientdata = 0;
        currentSelectedDropdownValuePatientSearch = 0;

        interfaceButtonsView = new Button[4] { btnEditPatient, btnSelectPatient, btnDeletePatient, btnAddPatient };
        interfaceButtonsChoose = new Button[3] { btnChooseHome, btnChoosePark, btnCancelChoose };
        allInputs = new InputField[6] { inputNumber, inputName, inputLevel, inputBirthday, inputGender, inputExtra };
    }
    void Update()
    {
        if (addPatientInterface.activeInHierarchy)
        {
            toggleTabAddPatient();
        }
        else if (patientViewInterface.activeInHierarchy)
        {
            toggleTabViewData();
        }
        else if (chooseSceneInterface.activeInHierarchy)
        {
            toggleTabChooseProject();
        }
    }

    private void toggleTabAddPatient()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            addPatientSelectedInput++;
            if (addPatientSelectedInput < allInputs.Length)
            {
                allInputs[addPatientSelectedInput].Select();
            }
            else if (addPatientSelectedInput < allInputs.Length + 1)
            {
                btnConfirmData.Select();
            }
            else if (addPatientSelectedInput < allInputs.Length + 2)
            {
                btnCancelData.Select();
            }
            else
            {
                addPatientSelectedInput = 0;
                allInputs[addPatientSelectedInput].Select();
            }
        }
    }

    private void toggleTabViewData()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {

            viewPatientSelectedInput++;

            if (viewPatientSelectedInput < 4)
            {
                Debug.Log(viewPatientSelectedInput);
                interfaceButtonsView[viewPatientSelectedInput].Select();
            }
            else if (viewPatientSelectedInput == 4)
            {
                inputSearch.Select();
            }
            else if (viewPatientSelectedInput == 5)
            {
                patientSearch.Select();
             /*   if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    Debug.Log(currentSelectedDropdownValuePatientdata);
                    currentSelectedDropdownValuePatientdata--;
                    if (currentSelectedDropdownValuePatientdata < 0)
                    {
                        currentSelectedDropdownValuePatientdata = possiblePatientSelect.options.Count;
                    }
                    possiblePatientSelect.RefreshShownValue();
                    possiblePatientSelect.value = currentSelectedDropdownValuePatientdata;
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    currentSelectedDropdownValuePatientdata++;
                    if (!(currentSelectedDropdownValuePatientdata < possiblePatientSelect.options.Count))
                    {
                        currentSelectedDropdownValuePatientdata = 0;
                    }
                    possiblePatientSelect.RefreshShownValue();
                    possiblePatientSelect.value = currentSelectedDropdownValuePatientdata;
                }*/
                
            }
            else if (viewPatientSelectedInput == 6)
            {
                possiblePatientSelect.Select();
            }
            else if (viewPatientSelectedInput > 6)
            {
                viewPatientSelectedInput = 0;
                interfaceButtonsView[viewPatientSelectedInput].Select();
            }
        }


    }

    private void toggleTabChooseProject()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            chooseProjectSelectedInput++;
            if (chooseProjectSelectedInput < interfaceButtonsChoose.Length)
            {
                interfaceButtonsChoose[chooseProjectSelectedInput].Select();
            }
            else
            {
                chooseProjectSelectedInput = 0;
                interfaceButtonsChoose[chooseProjectSelectedInput].Select();
            }

        }
    }

    private void showStartScreen()
    {
        addPatientInterface.SetActive(false);
        patientViewInterface.SetActive(true);
        chooseSceneInterface.SetActive(false);
        txtError.gameObject.SetActive(false);
    } //shows the interface with user data

    public void changeInterfaceToAdd()
    {
        patientViewInterface.SetActive(false);
        addPatientInterface.SetActive(true);
        addPatientSelectedInput = 0;
        allInputs[addPatientSelectedInput].Select();

    } //to open interface for adding data to user

    public void btnPatientAddClicked()
    {
        patientsToSort.Clear();
        clearInputs();
        foreach (profile patient in Patients)
        {
            patientsToSort.Add(patient.UserNumber);
        }
        patientCountToString = patientCount.ToString();
        while (patientsToSort.Contains(patientCountToString)) //check if the patient number is already in system
        {
            patientCount++;
            patientCountToString = patientCount.ToString();
        }
        inputNumber.text = patientCountToString;
        changeInterfaceToAdd();
    } //to add a patient from scratch

    public void btnPatientEditClicked()
    {
        editScreen = true;
        foreach (profile patientDetailsToShow in Patients)
        {
            if (patientDetailsToShow.patientSelectCheck(patientsToAdd[possiblePatientSelect.value])) //search the selected user and add the data 
            {
                inputNumber.text = patientDetailsToShow.UserNumber;
                inputBirthday.text = patientDetailsToShow.UserBirthday;
                inputLevel.text = patientDetailsToShow.UserSkill;
                inputName.text = patientDetailsToShow.UserName;
                inputExtra.text = patientDetailsToShow.UserExtraInfo;
                inputGender.text = patientDetailsToShow.UserGender;
            }
        }
        changeInterfaceToAdd();
    } //to edit a patients data

    public void btnChoosePatientClicked()
    {

        chooseSceneInterface.SetActive(true);
        patientViewInterface.SetActive(false);
        addPatientInterface.SetActive(false);
        foreach (profile patientDetailsToShow in Patients)
        {
            if (patientDetailsToShow.patientSelectCheck(patientsToAdd[possiblePatientSelect.value])) //search the selected user and select him
            {
                patientSelected = patientDetailsToShow;
            }
        }
        PlayerPrefs.SetInt("numberOfTimesPlayedHome", patientSelected.timesInHomeScene);
        PlayerPrefs.SetInt("numberOfTimesPlayedPark", patientSelected.timesInParkScene);
    } //switches interface and selects correct user

    public void btnPatientDeleteClicked()
    {
        patientsToChange.Clear();
        foreach (profile patientToCheck in Patients)
        {
            if (!(patientToCheck.patientSelectCheck(patientsToAdd[possiblePatientSelect.value]))) //add all users except selected, then rewrite data
            {
                patientsToChange.Add(patientToCheck);
            }
        }
        Patients.Clear();
        Patients.AddRange(patientsToChange);
        possiblePatientSelect.value--;
        writePatientData();
        showPatientData();
    } //delete a user from the list 

    public void btnConfirmPatientAddClicked()
    {

        if (editScreen) //if the user is edited, delete old data
        {
            btnPatientDeleteClicked();
        }
        if (inputNumber.text == "" || inputBirthday.text == "" || inputName.text == "" || inputLevel.text == "" || inputGender.text == "")
        {
            txtError.gameObject.SetActive(true);
        }
        else
        {
            txtError.gameObject.SetActive(false);
            Patients.Add(new profile(inputNumber.text, inputName.text, inputLevel.text, inputBirthday.text, inputExtra.text, "0", "0", inputGender.text));
            writePatientData();
            showPatientData();
            showPatientDetails();
            showStartScreen();
            editScreen = false;
            clearInputs();
        }

    } //confirms the data the user filled in

    private void clearInputs()
    {
        inputBirthday.text = "";
        inputExtra.text = "";
        inputLevel.text = "";
        inputName.text = "";
        inputNumber.text = "";
        inputGender.text = "";
    } //deletes input fields

    public void btnCancelClicked()
    {
        clearInputs();
        editScreen = false;
        showStartScreen();
        showPatientData();
    } //returns to home screen

    public void btnParkClicked()
    {
        patientSelected.playingPark();
        writePatientData();
        SceneManager.LoadScene("scene_park");
    } //starts park scene + counts to user

    public void btnHomeClicked()
    {
        patientSelected.playingHome();
        writePatientData();
        SceneManager.LoadScene("scene_home");
    } //starts home scene + counts to user

    private void readPatientData()
    {
        if (File.Exists(readWritePath))
        {
            try
            {
                sReader = File.OpenText(readWritePath);
                string userData = sReader.ReadLine();
                patientCount = 0;
                while (userData != null)
                {
                    dataSplitted = userData.Split(';');
                    Patients.Add(new profile(dataSplitted[0], dataSplitted[1], dataSplitted[2], dataSplitted[3], dataSplitted[4], dataSplitted[5], dataSplitted[6], dataSplitted[7]));
                    userData = sReader.ReadLine();
                    patientCount++;
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
        else
        {
            patientsToAdd.Clear();
            patientsToAdd.Add("Niemand Gevonden");
            possiblePatientSelect.AddOptions(patientsToAdd);
        }

    } //reads the file and places it in a list of profiles

    public void showPatientDetails()
    {
        foreach (profile patientDetailsToShow in Patients)
        {
            if (patientDetailsToShow.patientSelectCheck(patientsToAdd[possiblePatientSelect.value]))
            {
                txtProfileDetails.text = patientDetailsToShow.showPatientDetails();
            }
        }
    } //shows the details of the selected user

    public void showPatientData()
    {
        patientFound = false;
        if (inputSearch.text != "")
        {
            inputSearch.text = inputSearch.text.ToLower();
        }
        patientsToSort.Clear();
        patientsToAdd.Clear();
        possiblePatientSelect.ClearOptions();

        switch (patientSearch.value) //the value by which the users should be ordened
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
                    patientsToSort.Add(patient.UserBirthday);
                }
                break;
        }


        foreach (string patientDataToSort in patientsToSort) //using a recursive method to find the right users
        {
            if (inputSearch.text != "")
            {
                if (recursivePatientData(0, patientDataToSort))
                {

                    patientsToAdd.Add(patientDataToSort);
                    patientFound = true;
                }
            }
            else
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
        possiblePatientSelect.value = 0;
        showPatientDetails();
    } //show the right user (using the search)

    private bool recursivePatientData(int i, string patientDataToSort)
    {
        patientDataToSort = patientDataToSort.ToLower();
        if (i == inputSearch.text.Length || i == patientDataToSort.Length) //if the end of the word is reached
        {
            return true;
        }
        else
        {
            if (patientDataToSort[i] == inputSearch.text[i]) //compare letter of the words
            {
                i++;
                if (recursivePatientData(i, patientDataToSort))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    } //recursive method to find the right users

    private void writePatientData()
    {
        try
        {
            if (File.Exists(readWritePath)) //used for overwriting the file
            {
                File.Delete(readWritePath);
            }
            sWriter = File.CreateText(readWritePath);
            foreach (profile patientData in Patients)
            {
                sWriter.WriteLine(patientData.writePatientData());
            }
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
    ushort numberOfTimesInHomeScene;
    ushort numberOfTimesInParkScene;
    string gender;
    public profile() { }

    public profile(string tUserNumber, string tName, string tSkill, string tBirthday, string tExtraInfo, string tTimesInHome, string tTimesInPark, string tGender)
    {
        userNumber = tUserNumber;
        name = tName;
        skill = tSkill;
        birthday = tBirthday;
        extraInfo = tExtraInfo;
        gender = tGender;
        numberOfTimesInHomeScene = ushort.Parse(tTimesInHome);
        numberOfTimesInParkScene = ushort.Parse(tTimesInPark);
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
    public string UserGender
    {
        get { return gender; }
    }

    public ushort timesInHomeScene
    {
        get { return numberOfTimesInHomeScene; }
    }

    public ushort timesInParkScene
    {
        get { return numberOfTimesInParkScene; }
    }

    public void playingPark()
    {
        numberOfTimesInParkScene++;
    }
    public void playingHome()
    {
        numberOfTimesInHomeScene++;
    }

    // methods
    public string writePatientData()
    {
        return userNumber + ';' + name + ';' + skill + ";" + birthday + ";" + extraInfo + ";" + numberOfTimesInHomeScene + ";" + numberOfTimesInParkScene + ";" + gender;
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
        return "Patiëntnummer: " + userNumber + "\nPatiëntnaam: " + name + "\nNiveau: niveau " + skill + "\nGeboortedatum: " + birthday + "\nGeslacht: " + gender + "\nAantal keer in het park: " + numberOfTimesInParkScene + "\nAantal keer in het huis: " + numberOfTimesInHomeScene + "\nExtra info: " + extraInfo;
    }

} //profile objects
