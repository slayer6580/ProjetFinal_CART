using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiscountDelirium
{
    public class TowerPhysic : MonoBehaviour
    {
        //[field: SerializeField] private Rigidbody CharacterRB { get; set; } = null;
        [field: SerializeField] private GameObject Character { get;  set; } = null;
        [field: SerializeField] private GameObject CartPrefab { get; set; } = null;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                //Box1 currentTopBox = m_boxesInCart.ToArray()[0];
                //currentTopBox.RemoveItemImpulse();
                Debug.Log("Add left force to cart");
                // instanciate new gameobject
                GameObject instant = Instantiate(CartPrefab, Character.transform.position + new Vector3(-1,0,0), Character.transform.rotation * Quaternion.Euler(0, 90, 0));
                //GetComponentInParent<Rigidbody>().AddForce(Vector3.left, ForceMode.Impulse);
            }
            else if (Input.GetKeyDown(KeyCode.P))
            {
                //RemoveBoxImpulse();
                Debug.Log("Add right force to cart");
                //GetComponentInParent<Rigidbody>().AddForce(Vector3.right, ForceMode.Impulse);
            }
        }
    }
}
