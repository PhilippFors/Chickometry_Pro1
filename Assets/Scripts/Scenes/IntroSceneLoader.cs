using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroSceneLoader : MonoBehaviour
{
    [SerializeField] private VideoPlayer player;
    
    void Start()
    {
        StartCoroutine(WaitForIntro());
    }

    private IEnumerator WaitForIntro()
    {
        yield return new WaitForSeconds(0.5f);
        
        yield return new WaitUntil(() => !player.isPlaying);

        yield return new WaitForSeconds(1f);

        SceneManager.LoadSceneAsync("Tutorial", LoadSceneMode.Single);
    }
}
