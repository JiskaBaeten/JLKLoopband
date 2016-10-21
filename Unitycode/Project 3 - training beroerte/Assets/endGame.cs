using UnityEngine;
using UnityEngine.SceneManagement;

public class endGame : MonoBehaviour {

    bool confirmEscapeShowed = false;
    public GameObject escapeWarningInterface;
    void Start()
    {
        escapeWarningInterface.SetActive(false);
    }
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) //if playing, return to startscreen, otherwise stop the application
        {
            if (confirmEscapeShowed)
            {
                exitScene();       
            }
            else
            {
                escapeWarningInterface.SetActive(true);
            }
            
        }

	}
    public void exitScene()
    {
        escapeWarningInterface.SetActive(false);
        if (SceneManager.GetActiveScene().name == "StartScene")
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene("StartScene");
        }

    }
    public void btnCancelExit()
    {
        escapeWarningInterface.SetActive(false);
    }
}
