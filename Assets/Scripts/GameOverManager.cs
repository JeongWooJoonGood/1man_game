using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public void Retry()
    {
        SceneManager.LoadScene("GameScenes");
    }

    public void GoToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }
}