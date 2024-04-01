using UnityEngine;

[CreateAssetMenu(fileName = "itemData", menuName = "item/item data")]

public class ItemData : ScriptableObject
{
    public enum ESize
    {
        small,
        medium,
        large
    }

    public string m_name;
    public int m_cost;
    public ESize m_size;
    public Vector3 m_scaleInBox;

}
