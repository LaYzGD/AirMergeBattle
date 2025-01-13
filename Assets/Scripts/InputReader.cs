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

        _onClickAction.started += OnClickStarted;
        _onClickAction.canceled += OnClickCanceled;
        _onClickAction.performed += OnClickPerformed;
    }

    private void OnClickStarted(InputAction.CallbackContext context)
    {
        var raycastHit = Physics2D.Raycast(MousePosition, Vector2.zero, Mathf.Infinity);
        if (raycastHit.collider != null && raycastHit.collider.TryGetComponent(out IDragable dragable))
        {
            dragable.OnDragStart();
        }
    }

    private void OnClickPerformed(InputAction.CallbackContext context)
    {
        var raycastHit = Physics2D.Raycast(MousePosition, Vector2.zero, Mathf.Infinity);
        if (raycastHit.collider != null && raycastHit.collider.TryGetComponent(out IClickable clickable))
        {
            clickable.OnClick();
        }
    }

    private void OnClickCanceled(InputAction.CallbackContext context) 
    {
        var raycastHit = Physics2D.Raycast(MousePosition, Vector2.zero, Mathf.Infinity);
        if (raycastHit.collider != null && raycastHit.collider.TryGetComponent(out IDragable dragable))
        {
            dragable.OnDragEnd();
        }
    }

    private void OnDestroy()
    {
        _onClickAction.started -= OnClickStarted;
        _onClickAction.canceled -= OnClickCanceled;
        _onClickAction.performed -= OnClickPerformed;
    }
}
