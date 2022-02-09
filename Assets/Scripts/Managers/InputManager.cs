using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public Action<Vector2> OnLeftKey; // A
    public Action OnLeftKeyUp;

    public Action<Vector2> OnRightKey; // D
    public Action OnRightKeyUp;

    public Action OnJumpKeyDown; // W
    public Action OnJumpKeyUp;

    public Action OnSpaceKeyDown; // Space - Roll

    public Action<Define.MouseEvent> MouseAction = null;

    public Vector3 CursorPos { get { return Camera.main.ScreenToWorldPoint(Input.mousePosition); } }



    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update called in GameManager
    public void OnUpdate()
    {
        #region WASD Key
        if (Input.GetKeyDown(KeyCode.W))
        {
            OnJumpKeyDown?.Invoke();
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            OnJumpKeyUp?.Invoke();
        }

        if (Input.GetKey(KeyCode.A))
        {
            OnLeftKey?.Invoke(Vector2.left);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            OnLeftKeyUp?.Invoke();
        }

        if (Input.GetKey(KeyCode.D))
        {
            OnLeftKey?.Invoke(Vector2.right);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            OnRightKeyUp?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnSpaceKeyDown?.Invoke();
        }
        #endregion

        #region MouseClick
        if (Input.GetMouseButtonDown(0))
        {
            MouseAction?.Invoke(Define.MouseEvent.LClick);
        }
        if (Input.GetMouseButtonDown(1))
        {
            MouseAction?.Invoke(Define.MouseEvent.RClick);
        }
        #endregion






    }
}
