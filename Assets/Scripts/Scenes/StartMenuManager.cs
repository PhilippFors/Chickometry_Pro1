using Services;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    public bool skipIntro;

    [SerializeField] private ApplicationManager applicationManager;

    public void StartGame()
    {
        if (skipIntro) {
            LoadScene("Tutorial");
        }
        else {
            LoadScene("IntroSequence");
        }
    }

    public void QuitGame()
    {
        applicationManager.Quit();
    }
    
    private void LoadScene(string n)
    {
        SceneManager.LoadScene(n, LoadSceneMode.Single);
    }

    public void SkipIntro()
    {
        skipIntro = !skipIntro;
    }
}
