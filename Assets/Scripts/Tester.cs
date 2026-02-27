using UnityEngine;
using UnityEngine.SceneManagement;

public class Tester : MonoBehaviour
{
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}