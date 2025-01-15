using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    private void Start()
    {
        SaveAndLoad.Load();
        Application.targetFrameRate = 60;
    }
}
