using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
public class InputController : MonoBehaviour
{
    InputControls input;
    public EventHandler OnClickedLeftClick;
    
    private void Awake()
    {
        input = new InputControls();
        input.Enable();

        input.MouseInput.Left_Click.performed += x => { OnClickedLeftClick?.Invoke(this, EventArgs.Empty); };
    }
    public Vector2 GetMousePos()
    {
        Vector2 mousPos = Camera.main.ScreenToWorldPoint(input.MouseInput.Position.ReadValue<Vector2>());
        return mousPos;
    }
}
