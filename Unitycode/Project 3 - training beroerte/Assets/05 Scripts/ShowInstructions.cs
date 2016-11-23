using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShowInstructions : MonoBehaviour
{
  //to show a tutorial when a new player plays the game

  //on that note, read how many times a player played or it will always start!!!!!

  RawImage instructionImage = null;
  public Texture instrucHomeText;
  public Texture instrucHomeFindDog;
  public Texture instrucChoosePathText;
  public Texture instrucChoosePathHowTo;
  public Texture instrucCallDog;
  public Texture instrucArrows;

  private Texture[] arrInstrucScene_Home = new Texture[2];
  private Texture[] arrInstrucScene_Park = new Texture[2];
  private byte i = 0; //to count the instructions
  public bool isDogRunningAway = false;

  private Scene currentScene;
  private string sceneName = "";

  void Start()
  {
    i = 0; //to make sure it's always zero when started
    currentScene = SceneManager.GetActiveScene(); //get the currentScene
    sceneName = currentScene.name; //get the name of the currentscene

    instructionImage = (RawImage)this.GetComponent<RawImage>(); //look for the component to change

    arrInstrucScene_Home[0] = instrucHomeText;
    arrInstrucScene_Home[1] = instrucHomeFindDog;

    arrInstrucScene_Park[0] = instrucChoosePathText;
    arrInstrucScene_Park[1] = instrucChoosePathHowTo;

    if (sceneName == "scene_home")
    {
            if (PlayerPrefs.GetInt("numberOfTimesPlayedHome") == 0)
            {
                PlayerPrefs.DeleteKey("numberOfTimesPlayedHome");
                instructionImage.texture = (Texture)instrucArrows;
                instructionImage.texture = (Texture)arrInstrucScene_Home[0]; //just to make sure this is the first to show
            }
      
      
    }
    else if (sceneName == "scene_park")
    {
            if (PlayerPrefs.GetInt("numberOfTimesPlayedPark") == 0)
            {
                PlayerPrefs.DeleteKey("numberOfTimesPlayedHome");
                instructionImage.texture = (Texture)arrInstrucScene_Park[0]; //just to make sure this is the first to show
            }

    }
  }

  void LateUpdate()
  {
    if (isDogRunningAway)
    {
      instructionImage.color = new Color(255, 255, 255, 1); //set visible again
      instructionImage.texture = (Texture)instrucCallDog;
    }

    if (Input.GetKeyDown("enter")) //if enter is pressed => has to change to VIVE at one point
    {
      if (sceneName == "scene_home")
      {
        if (i < arrInstrucScene_Home.Length - 1)
        {
          i++;
          instructionImage.texture = (Texture)arrInstrucScene_Home[i];
        }
        else if (i >= arrInstrucScene_Home.Length - 1)//if no more instructions left
        {
          instructionImage.color = new Color(255, 255, 255, 0); //invisible, otherwise white block
          instructionImage.texture = null; //empty cache
        }
      }
      else if (sceneName == "scene_park")
      {
        if (i < arrInstrucScene_Park.Length - 1)
        {
          i++;
          instructionImage.texture = (Texture)arrInstrucScene_Park[i];
        }
        else if (i >= arrInstrucScene_Park.Length - 1 || isDogRunningAway == true)//if no more instructions left, or the dog is running instruction was active
        {
          isDogRunningAway = false;
          instructionImage.color = new Color(255, 255, 255, 0); //invisible, otherwise white block
          instructionImage.texture = null; //empty cache
        }
      }
    }
  }
}