using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputReader : MonoBehaviour
{
    [SerializeField] private string _selectActionName = "Point";
    [SerializeField] private string _clickActionName = "Click";
    [SerializeField] private Camera _mainCam;

    private PlayerInput _playerInput;
    private InputAction _onSelectAction;
    private InputAction _onClickAction;
    public event Action Click;

    public Vector2 MousePosition => _mainCam.ScreenToWorldPoint(_onSelectAction.ReadValue<Vector2>());

    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _onSelectAction = _playerInput.actions[_selectActionName];
        _onClickAction= _playerInput.actions[_clickActionName];

        _onClickAction.performed += OnClick;
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        Click?.Invoke();
    }

    private void OnDestroy()
    {
        _onClickAction.performed -= OnClick;
    }
}
