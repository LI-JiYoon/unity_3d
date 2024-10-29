using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    [Header("Movement")]
    public float moveSpeed;
    private Vector2 curMovementInput;
    public float jumptForce;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;
    public LayerMask raycastLayerMask;
    public Text itemName;

    private Vector2 mouseDelta;

    [HideInInspector]
    public bool canLook = true;

    public Action inventory;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //마우스 커서 숨기기
    }

   
    private void FixedUpdate()
    {
        Move();
    }
    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    private void OnCollisionEnter(Collision collision)//점프대네서 점프
    {
        if (collision.gameObject.tag == "Jump Panel")
        {
            _rigidbody.AddForce(force: Vector3.up * 100f, mode: ForceMode.Impulse);
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)//키가 눌렸을때
        {
            curMovementInput = context.ReadValue<Vector2>();//벡터값 가져오기
        }
        else if (context.phase == InputActionPhase.Canceled)//키가 떨어졌을때
        {
            curMovementInput = Vector2.zero;//움직임x
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded()) // 땅에 붙어 있을 때만 점프
        {
            _rigidbody.AddForce(Vector2.up * jumptForce, ForceMode.Impulse);
        }
    }

    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;//상하 좌우 값
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y; // 점프를 했을 때 위아래로만 움직여야하기 때문에 그 값을 유지

        _rigidbody.velocity = dir;
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity; //마우스 좌우 * 마우스 민감도
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook); //최소 최대값
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0); //로컬좌표

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);//위아래(아래를 보면 x가 +가 되어야하기 때문에 부호반전)
    }

    bool IsGrounded()// 땅에 붙어있는지
    {
        Ray[] rays = new Ray[4]//다리4개
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down), //z축 앞
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),//z축 뒤
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),//x축 오
            new Ray(transform.position + (-transform.right * 0.2f) +(transform.up * 0.01f), Vector3.down)//x축 왼 
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))// ray, ray길이,충돌을 감지할 때 고려할 레이어(즉, 지면 레이어)를 지정하여 지면 외의 요소와의 충돌을 무시
            {
                return true;
            }
        }

        return false;
    }
    

    public void OnInventoryButton(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started)//tab키 한번 눌렀을때
        {
            inventory?.Invoke();//등록한 toggle 호출
            ToggleCursor();
        }
    }
    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;//
        canLook = !toggle;
    }
}
