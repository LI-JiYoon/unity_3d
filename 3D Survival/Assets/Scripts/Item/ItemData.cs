using UnityEngine;

public enum ItemType
{
    Resource,
    Equipable,
    Consumable
}

public enum ConsumableType
{
    Hunger,
    Health
}

[System.Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")] //아이템이 같은 종류로 여러개일 경우
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")]//체력/배고픔 구분
    public ItemDataConsumable[] consumables;

    [Header("Equip")]//장착
    public GameObject equipPrefab;
}