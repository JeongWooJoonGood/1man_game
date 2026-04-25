using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public void StartGame()
    {
        // 게임 씬으로 이동 (씬 이름은 본인 게임 씬 이름)
        SceneManager.LoadScene("GameScenes");
    }

    public void QuitGame()
    {
        Debug.Log("게임 종료");
        Application.Quit();

        // 에디터에서 테스트용
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}