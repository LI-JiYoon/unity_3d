using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IInteractable
{
    public string GetInteractPrompt();//화면에 띄워줃 내용
    public void OnInteract();//상호작용 시 효과
}

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;//확인빈도
    private float lastCheckTime;//마지막 확인 시점
    public float maxCheckDistance;//확인거리
    public LayerMask layerMask;//어떤 layer를 검출할지

    public GameObject curInteractGameObject;//현재 탐지한 객체
    private IInteractable curInteractable;//인터페이스 캐싱

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

            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));//정중앙에서 ray 쏨
            RaycastHit hit;//부딪힌 오브젝트 정보

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
        if (context.phase == InputActionPhase.Started && curInteractable != null)//키를 눌렸을때, 에임이 아이템을 바라보고 있을때
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}