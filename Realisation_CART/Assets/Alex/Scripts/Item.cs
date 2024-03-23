using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData m_data;

    // pour le slerp
    private Transform m_target;
    private bool m_isMoving = false;


    public void GoInsideBoxSlot(Transform target)
    {
        // TODO l'objet doit Lerp vers le transform du slot.
    }
}
