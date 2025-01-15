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
    public bool IsUICover { get; set; }

    public Vector2 MousePosition => _mainCam.ScreenToWorldPoint(_onSelectAction.ReadValue<Vector2>());

    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _onSelectAction = _playerInput.actions[_selectActionName];
        _onClickAction= _playerInput.actions[_clickActionName];

        _onClickAction.canceled += OnClickCanceled;
        _onClickAction.performed += OnClickPerformed;
    }

    private void OnClickPerformed(InputAction.CallbackContext context)
    {
        if (IsUICover)
        {
            return;
        }

        var raycastHit = Physics2D.Raycast(MousePosition, Vector2.zero, Mathf.Infinity);
        if (raycastHit.collider != null && raycastHit.collider.TryGetComponent(out IClickable clickable))
        {
            clickable.OnClick();
        }
        if (raycastHit.collider != null && raycastHit.collider.TryGetComponent(out IDragable dragable))
        {
            dragable.OnDragStart();
        }
    }

    private void OnClickCanceled(InputAction.CallbackContext context) 
    {
        if (IsUICover)
        {
            return;
        }

        var raycastHit = Physics2D.Raycast(MousePosition, Vector2.zero, Mathf.Infinity);
        if (raycastHit.collider != null && raycastHit.collider.TryGetComponent(out IDragable dragable))
        {
            dragable.OnDragEnd();
        }
    }

    private void OnDestroy()
    {
        _onClickAction.canceled -= OnClickCanceled;
        _onClickAction.performed -= OnClickPerformed;
    }
}
