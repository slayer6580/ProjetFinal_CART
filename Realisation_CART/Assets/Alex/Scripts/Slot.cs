using UnityEngine;

public class Slot : MonoBehaviour
{
    /// <summary> Sert a tester le placement de slot avec le prefab SlotTest </summary>
    private void Start()
    {
        if (transform.childCount == 1)
        {
            int randomColor = Random.Range(0, 5);
            transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color = GetRandomColor(randomColor);
        }
    }

    private void Update()
    {
        // Sert a tester le placement de slot avec rendu au gameplay (Scene)
        //Debug.Log("transform.parent.parent.position " + transform.parent.parent.position);
        Vector3 slotPosition = transform.parent.parent.position + transform.localPosition;
        //transform.rotation = transform.parent.parent.rotation;

        Debug.DrawLine(transform.position, transform.position + Vector3.up, Color.black);
    }

    //private void Update()
    //{
    //    // Sert a tester le placement de slot avec rendu au gameplay (Scene)
    //    //Debug.Log("transform.parent.parent.position  " + transform.parent.parent.position);
    //    //Debug.Log("transform.parent.parent.name  " + transform.parent.parent.name);
    //    //Debug.Log("transform.name  " + transform.name);
    //    Vector3 slotPosition = transform.parent.parent.parent.position + transform.localPosition;
    //    //Debug.Log("slotPosition  " + slotPosition);
    //    //transform.rotation = transform.parent.parent.rotation;
    //    Debug.DrawLine(slotPosition, slotPosition + Vector3.up, Color.black);
    //}

    /// <summary> Donne une couleur random a la slot spécial </summary>
    Color GetRandomColor(int value)
    {
        return new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1) ;
    }

}
