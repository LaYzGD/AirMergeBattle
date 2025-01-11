using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Observer<T>
{
    [SerializeField] private T _value;
    [SerializeField] private UnityEvent<T> _onValueChanged;

    public T Value
    {
        get
        {
            return _value;
        }
        set
        {
            Set(value);
        }
    }

    public Observer(T value, UnityAction<T> callback = null)
    {
        _value = value;
        _onValueChanged = new UnityEvent<T>();
        if (callback != null)
        {
            _onValueChanged.AddListener(callback);
        }
    }

    public void Set(T value, bool shouldNotify = true)
    {
        if (Equals(_value, value))
        {
            return;
        }

        _value = value;
        
        if (shouldNotify) 
        {
            Invoke();
        }
    }

    public void Invoke()
    {
        _onValueChanged?.Invoke(_value);
    }

    public void AddListener(UnityAction<T> callback) 
    {
        if (callback == null)
        {
            return;
        }
        if (_onValueChanged == null)
        {
            _onValueChanged = new UnityEvent<T>();
        }
        _onValueChanged.AddListener(callback);
    }

    public void RemoveListener(UnityAction<T> callback)
    {
        if (callback == null)
        {
            return;
        }
        if (_onValueChanged == null)
        {
            return;
        }
        _onValueChanged.RemoveListener(callback);
    }

    public void RemoveAllListeners()
    {
        if (_onValueChanged == null)
        {
            return;
        }

        _onValueChanged.RemoveAllListeners();
    }

    public void Dispose()
    {
        RemoveAllListeners();
        _onValueChanged = null;
        _value = default;
    }
}
