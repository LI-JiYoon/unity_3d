using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IInteractable
{
    public string GetInteractPrompt();//ȭ�鿡 ����� ����
    public void OnInteract();//��ȣ�ۿ� �� ȿ��
}

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;//Ȯ�κ�
    private float lastCheckTime;//������ Ȯ�� ����
    public float maxCheckDistance;//Ȯ�ΰŸ�
    public LayerMask layerMask;//� layer�� ��������

    public GameObject curInteractGameObject;//���� Ž���� ��ü
    private IInteractable curInteractable;//�������̽� ĳ��

    public TextMeshProUGUI promptText;
    private Camera camera;

    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));//���߾ӿ��� ray ��
            RaycastHit hit;//�ε��� ������Ʈ ����

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    SetPromptText();
                }
            }
            else
            {
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }

    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractPrompt();
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)//Ű�� ��������, ������ �������� �ٶ󺸰� ������
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}