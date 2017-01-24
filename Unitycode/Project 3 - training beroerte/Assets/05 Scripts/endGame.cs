using UnityEngine;
using UnityEngine.SceneManagement;

//used everywhere to return to the homescene (PatientIO) or to quit the game
public class endGame : MonoBehaviour {

	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) //if playing, return to startscreen, otherwise stop the application
        {
                exitScene();              
        }
	}
    public void exitScene()
    {
        if (SceneManager.GetActiveScene().name == "StartScene") //at startscene, quit application
        {
            Application.Quit();
        }
        else //otherwise go back to startscene
        {
            SceneManager.LoadScene("StartScene");
        }

    }
}
