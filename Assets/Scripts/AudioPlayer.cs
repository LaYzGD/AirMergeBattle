using UnityEngine;
using UnityEngine.Pool;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioObject _audioObjectPrefab;
    [SerializeField] private float _minPitch = 0.9f;
    [SerializeField] private float _maxPitch = 1.2f;

    private ObjectPool<AudioObject> _audioPool;


    private void Awake()
    {
        _audioPool = new ObjectPool<AudioObject>(OnCreate, OnGet, OnRelease, OnAudioDestroy, false);
    }

    public void PlaySound(AudioClip sound, float volume)
    {
        var audioObj = _audioPool.Get();
        audioObj.Init(sound, volume, UnityEngine.Random.Range(_minPitch, _maxPitch), KillAudioObject);
        audioObj.PlaySound();
    }

    private void KillAudioObject(AudioObject audio) 
    {
        _audioPool.Release(audio);
    }

    private AudioObject OnCreate()
    {
        return Instantiate(_audioObjectPrefab);
    }

    private void OnGet(AudioObject audio)
    {
        audio.gameObject.SetActive(true);
    }

    private void OnRelease(AudioObject audio)
    {
        audio.gameObject.SetActive(false);
    }

    private void OnAudioDestroy(AudioObject audio)
    {
        Destroy(audio.gameObject);
    }
}
