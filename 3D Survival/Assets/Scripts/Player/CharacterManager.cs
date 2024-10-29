// ������� �ʴ� using�� ����
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
            DontDestroyOnLoad(gameObject); // �� �̵��ص� ���� ����
        }
        else
        {
            if (_instance != this) // ���� �ν��Ͻ��� this�� �ٸ��ٸ�
            {
                Destroy(gameObject); //������� �ı�
            }
        }
    }






}