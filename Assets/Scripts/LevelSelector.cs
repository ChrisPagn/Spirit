using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public void LoadLevelPassed(string LevelName)
    {
        SceneManager.LoadScene(LevelName);
    }
}
