using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(this);

        SaveAndLoad.Load();
        Application.targetFrameRate = 60;
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            SaveAndLoad.Save();
            return;
        }

        SaveAndLoad.Load();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveAndLoad.Save();
            return;
        }

        SaveAndLoad.Load();
    }
}
