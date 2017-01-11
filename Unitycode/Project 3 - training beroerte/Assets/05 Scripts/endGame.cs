using UnityEngine;
using UnityEngine.SceneManagement;

public class endGame : MonoBehaviour {

    void Start()
    {
        Debug.Log(gameObject.name);
    }
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) //if playing, return to startscreen, otherwise stop the application
        {
                exitScene();              
        }

	}
    public void exitScene()
    {
        if (SceneManager.GetActiveScene().name == "StartScene")
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene("StartScene");
        }

    }
}
