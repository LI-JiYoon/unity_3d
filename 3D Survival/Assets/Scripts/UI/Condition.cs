using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float curValue;//현재 상태값
    public float maxValue;//상태 최대값
    public float startValue;//시작 상태값
    public float passiveValue;//주기적으로 변화하는 값
    public Image uiBar;//fill amount 조절(Image 바 길이)

    private void Start()
    {
        curValue = startValue;
    }

    private void Update()
    {
        //UI 업데이트
        uiBar.fillAmount = GetPercentage();
    }

    public void Add(float amount)//회복
    {
        curValue = Mathf.Min(curValue + amount, maxValue);//최대회복값 정해주기
    }

    public void Subtract(float amount)//감소
    {
        curValue = Mathf.Max(curValue - amount, 0.0f);//최소값 정해주기
    }

    public float GetPercentage()
    {
        return curValue / maxValue;
    }
}