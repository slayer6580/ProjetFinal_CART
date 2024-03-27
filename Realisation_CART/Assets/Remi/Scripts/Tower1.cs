using DiscountDelirium;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace BoxSystem
{

    public class Tower1 : MonoBehaviour
    {
        [field: Header("Mettre le Cart GO ici")]
        [field: SerializeField] public GameObject Cart { get; private set; }
        [field: SerializeField] private GameObject DebugCartPrefab { get; set; } = null;
        [field: SerializeField] public GameObject Player { get; private set; }
        private TowerPhysics _TowerPhysics { get; set; } = null;

        [field: Header("La distance ou un item devrait snap a la boite pendant le slerp")]
        [field: SerializeField] public float ItemSnapDistance { get; private set; }

        [Header("Mettre le Prefab de la boite")]
        [SerializeField] private GameObject m_boxPrefab;
        [Header("La hauteur de placement de la boite")]
        [SerializeField] private float m_boxHeight;

        private int m_boxCount = 0;
        private Stack<Box1> m_boxesInCart = new Stack<Box1>();

        void Start()
        {
            _TowerPhysics = GetComponent<TowerPhysics>();
            AddBoxToTower();
        }

        /// <summary> Ajoute une boite a la tour </summary>
        public void AddBoxToTower()
        {
            m_boxCount++;

            // setup de la boite
            GameObject instant = Instantiate(m_boxPrefab);
            instant.transform.rotation = Player.transform.rotation;
            instant.transform.SetParent(transform);
            instant.name = "Box " + m_boxCount;
            Box1 instantBox = instant.GetComponent<Box1>();
            instantBox.SetTower(this);

            // hauteur de la boite dans la tour
            float height = (m_boxCount - 1) * m_boxHeight;
            instant.transform.localPosition = new Vector3(0, height, 0);

            // ajout a la liste
            m_boxesInCart.Push(instantBox);

            //_TowerPhysics.AddSpringToBox();  // Ta fonction ici, p-e rajouter instantBox en parametre
        }

        /// <summary> Enleve une boite a la tour </summary>
        public void RemoveBoxFromTower()
        {
            if (m_boxCount == 1)
            {
                Debug.LogWarning("¨On peut pas enlever toute les boites du panier");
                return;
            }
            //Debug.Log("RemoveBoxFromTower() m_boxCount: " + m_boxCount);
            m_boxCount--;
            Debug.Log("RemoveBoxFromTower() m_boxCount: " + m_boxCount);
            //m_boxesInCart.Pop();
            Box1 boxToRemove = m_boxesInCart.Pop();
            //Destroy(GetTopBox().gameObject);
            Destroy(boxToRemove.gameObject);
            //_TowerPhysics.ModifyTopBoxSpringIntesity(); // DÉPLASSER ICI
        }

        // TEST
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                AddBoxToTower();
            }
            else if (Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                RemoveBoxFromTower();
            }
            else if (Input.GetKeyDown(KeyCode.O))
            {
                Box1 currentTopBox = GetTopBox();
                currentTopBox.RemoveItemImpulse();
            }
            else if (Input.GetKeyDown(KeyCode.P))
            {
                _TowerPhysics.RemoveBoxImpulse();
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                Vector3 pos = Player.transform.position + transform.localPosition;
                Quaternion rot = transform.rotation;
                Quaternion additionalRotation = Quaternion.Euler(0, 90, 0);
                Quaternion finalRotation = rot * additionalRotation;

                Vector3 localOffset = new Vector3(-3, -1, 0);
                Vector3 worldOffset = rot * localOffset;

                Instantiate(DebugCartPrefab, pos + worldOffset, finalRotation);
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                Vector3 pos = Player.transform.position + transform.localPosition;
                Quaternion rot = transform.rotation;
                Quaternion additionalRotation = Quaternion.Euler(0, -90, 0);
                Quaternion finalRotation = rot * additionalRotation;

                Vector3 localOffset = new Vector3(3, -1, 0);
                Vector3 worldOffset = rot * localOffset;

                Instantiate(DebugCartPrefab, pos + worldOffset, finalRotation);
            }
        }

        /// <summary> Regarde si la boite du dessus pourrait prendre un objet d'une certaine taille </summary>
        public bool CanTakeObjectInTheActualBox(ItemData.ESize size)
        {
            return GetTopBox().CanPutItemInsideBox(size);
        }

        /// <summary> Pour donner un item a la boite du dessus </summary>
        public void PutObjectInTopBox(GameObject item)
        {
            GetTopBox().PutItemInBox(item);
        }

        /// <summary> Donne la boite du dessus </summary>
        public Box1 GetTopBox()
        {
            if (m_boxesInCart.Count == 0)
                return null;

            return m_boxesInCart.Peek();
        }

        /// <summary> Donne le nombre de boites dans le panier </summary>
        public int GetBoxCount()
        {
            return m_boxCount;
        }

        internal int GetBoxesCount()
        {
            return m_boxCount;
        }
    }
}
