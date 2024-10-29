using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float curValue;//���� ���°�
    public float maxValue;//���� �ִ밪
    public float startValue;//���� ���°�
    public float passiveValue;//�ֱ������� ��ȭ�ϴ� ��
    public Image uiBar;//fill amount ����(Image �� ����)

    private void Start()
    {
        curValue = startValue;
    }

    private void Update()
    {
        //UI ������Ʈ
        uiBar.fillAmount = GetPercentage();
    }

    public void Add(float amount)//ȸ��
    {
        curValue = Mathf.Min(curValue + amount, maxValue);//�ִ�ȸ���� �����ֱ�
    }

    public void Subtract(float amount)//����
    {
        curValue = Mathf.Max(curValue - amount, 0.0f);//�ּҰ� �����ֱ�
    }

    public float GetPercentage()
    {
        return curValue / maxValue;
    }
}