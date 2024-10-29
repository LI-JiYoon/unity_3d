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
        Cursor.lockState = CursorLockMode.Locked; //���콺 Ŀ�� �����
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

    private void OnCollisionEnter(Collision collision)//������׼� ����
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
        if (context.phase == InputActionPhase.Performed)//Ű�� ��������
        {
            curMovementInput = context.ReadValue<Vector2>();//���Ͱ� ��������
        }
        else if (context.phase == InputActionPhase.Canceled)//Ű�� ����������
        {
            curMovementInput = Vector2.zero;//������x
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded()) // ���� �پ� ���� ���� ����
        {
            _rigidbody.AddForce(Vector2.up * jumptForce, ForceMode.Impulse);
        }
    }

    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;//���� �¿� ��
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y; // ������ ���� �� ���Ʒ��θ� ���������ϱ� ������ �� ���� ����

        _rigidbody.velocity = dir;
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity; //���콺 �¿� * ���콺 �ΰ���
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook); //�ּ� �ִ밪
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0); //������ǥ

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);//���Ʒ�(�Ʒ��� ���� x�� +�� �Ǿ���ϱ� ������ ��ȣ����)
    }

    bool IsGrounded()// ���� �پ��ִ���
    {
        Ray[] rays = new Ray[4]//�ٸ�4��
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down), //z�� ��
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),//z�� ��
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),//x�� ��
            new Ray(transform.position + (-transform.right * 0.2f) +(transform.up * 0.01f), Vector3.down)//x�� �� 
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))// ray, ray����,�浹�� ������ �� ����� ���̾�(��, ���� ���̾�)�� �����Ͽ� ���� ���� ��ҿ��� �浹�� ����
            {
                return true;
            }
        }

        return false;
    }
    

    public void OnInventoryButton(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started)//tabŰ �ѹ� ��������
        {
            inventory?.Invoke();//����� toggle ȣ��
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
