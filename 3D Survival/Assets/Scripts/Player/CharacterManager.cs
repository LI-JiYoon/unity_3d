// 사용하지 않는 using문 삭제
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager _instance;
    public static CharacterManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("CharacterManager").AddComponent<CharacterManager>();


            }
            return _instance;
        }
    }
    public Player Player
    {
        get { return _player; }
        set { _player = value; }
    }
    private Player _player;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this; 
            DontDestroyOnLoad(gameObject); // 씬 이동해도 정보 유지
        }
        else
        {
            if (_instance != this) // 기존 인스턴스와 this가 다르다면
            {
                Destroy(gameObject); //현재것을 파괴
            }
        }
    }






}
