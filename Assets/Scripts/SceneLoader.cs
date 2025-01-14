using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private int _nextSceneIndex = 1;
    [SerializeField] private Image _loadingProgress;

    private float _target;

    private void Start()
    {
        LoadScene();
    }

    private void LoadScene() 
    {
        var scene = SceneManager.LoadSceneAsync(_nextSceneIndex);
        scene.allowSceneActivation = false;

        do
        {
            Task.Delay(100);
            _target = scene.progress;
        } while (scene.progress < 0.9f);

        scene.allowSceneActivation = true;
    }

    private void Update()
    {
        _loadingProgress.fillAmount = Mathf.MoveTowards(_loadingProgress.fillAmount, _target, 2 * Time.deltaTime);
    }
}
