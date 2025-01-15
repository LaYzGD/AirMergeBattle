using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(this);

        SaveAndLoad.Load();
        Application.targetFrameRate = 60;
    }

    private void OnApplicationQuit()
    {
        SaveAndLoad.Save();
    }
}
