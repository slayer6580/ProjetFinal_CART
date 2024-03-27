using BoxSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DiscountDelirium
{
    public class TowerPhysics : MonoBehaviour
    {
        private const int NB_UNDROPPABLE_BOXES = 4;

        private Tower1 _Tower { get; set; } = null;
        private bool m_isInHingeMode = false;

        private void Awake()
        {
            _Tower = GetComponent<Tower1>();
        }

        /// <summary> Retire une boite avec une force sur mesure</summary>
        public void RemoveBoxImpulse() // Rémi
        {
            // get the top item
            if (_Tower.GetBoxCount() <= 0)
            {
                Debug.LogWarning("No box on the stack");
                return;
            }

            Box1 topBox = _Tower.GetTopBox();

            Rigidbody boxRB = topBox.GetComponent<Rigidbody>();
            if (boxRB == null)
                boxRB = topBox.AddComponent<Rigidbody>();

            boxRB.AddForce(Vector3.left + Vector3.up * 10, ForceMode.Impulse);
            topBox.gameObject.GetComponent<AutoDestruction>().enabled = true;
        }

        /// <summary> Retire une boite avec une force provenant de l'exterieur </summary>
        public void RemoveBoxImpulse(Vector3 velocity)
        {
            Debug.Log("RemoveBoxImpulse()");
            Box1 topBox = _Tower.GetTopBox();
            if (topBox == null)
            {
                Debug.LogWarning("No box to remove");
                return;
            }

            topBox.GetComponent<Rigidbody>().isKinematic = false;
            topBox.GetComponent<BoxPhysics>().m_incomingVelocity = velocity;
            topBox.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.Impulse);
            Debug.Log("AutoDestruction enabled");
            topBox.GetComponent<AutoDestruction1>().enabled = true;
        }

        public void ModifyTopBoxSpringIntesity()
        {
            //if (m_boxCount > 2)
            //{
            //    Debug.Log("m_boxCount > 2: " + (m_boxesInCart.Count - 1));
            //    Box1 currentTopBox = m_boxesInCart.ToArray()[0];
            //    Debug.Log("previousBox: " + currentTopBox.name);
            //    SpringJoint previousSpringJoint = currentTopBox.GetComponent<SpringJoint>();
            //    previousSpringJoint.spring = 5;
            //}
        }

        public void AddJointToBox() // p-e rajouter instantBox en parametre    // Rémi
        {
            if (m_isInHingeMode)
            {
                AddHingeJoint();
                return;
            }

            //Debug.LogError("AddJointToBox()");
            AddSpringJoint();
        }

        private void AddHingeJoint()
        {
            // TODO: Should be done in the rigidbody in the prefab
            //if (_Tower.GetBoxCount() == 1) // Pour ajouter un spring entre la première boite et le panier
            //{
            //    Box1 box = _Tower.GetTopBox();
            //    box.GetComponent<Rigidbody>().isKinematic = true;
            //    return;
            //}

            //if (_Tower.GetBoxCount() > 1) // Pour ajouter un spring entre les boites
            //{
            //    Box1 box = _Tower.GetTopBox();
            //    box.GetComponent<Rigidbody>().isKinematic = true;
            //    return;
            //}

            //if (_Tower.GetBoxCount() > NB_UNDROPPABLE_BOXES) 
            //{
            //    Box1 box = _Tower.GetTopBox();
            //}
        }

        private void AddSpringJoint()
        {
            //Box1 box = _Tower.GetTopBox();
            //box.GetComponent<Rigidbody>().isKinematic = true;
            if (_Tower.GetBoxCount() == 1) // Pour ajouter un spring entre la première boite et le panier
            {
                //Rigidbody cartRB = GetComponentInParent<Rigidbody>();
                //if (cartRB == null) Debug.LogError("Cart n'a pas de rigidbody");
                //SpringJoint springJoint = m_boxesInCart.ToArray()[0].gameObject.AddComponent<SpringJoint>();
                //SetSprintJointValues(springJoint, cartRB);
                Box1 box1 = _Tower.GetTopBox();
                box1.GetComponent<Rigidbody>().isKinematic = true;
                return;
            }

            if (_Tower.GetBoxCount() > 1) // Pour ajouter un spring entre les boites
            {
                //Box1 previousBoxe = m_boxesInCart.ToArray()[m_boxesInCart.Count - 1];
                //SpringJoint springJoint = m_boxesInCart.ToArray()[0].gameObject.AddComponent<SpringJoint>();
                //Rigidbody newBoxeRB = previousBoxe.GetComponent<Rigidbody>();
                //if (newBoxeRB == null)
                //{
                //    newBoxeRB = previousBoxe.AddComponent<Rigidbody>();
                //}
                //SetSprintJointValues(springJoint, newBoxeRB);
                //Box1 box1 = GetTopBox();
                //box1.GetComponent<Rigidbody>().isKinematic = true;
                Box1 box1 = _Tower.GetTopBox();
                box1.GetComponent<Rigidbody>().isKinematic = true;
                return;
            }

            if (_Tower.GetBoxCount() > 2) // Pour changer la force du spring du top box
            {
                //Box1 previousBox = m_boxesInCart.ToArray()[0];
                //SpringJoint previousSpringJoint = previousBox.GetComponent<SpringJoint>();
                //previousSpringJoint.spring = 10;
            }
        }

        private static void SetSprintJointValues(SpringJoint springJoint, Rigidbody newBoxeRB) // Rémi
        {
            springJoint.connectedBody = newBoxeRB;
            springJoint.spring = 1;
            springJoint.damper = 0;
            springJoint.minDistance = 0;
            springJoint.maxDistance = 0.2f;
            springJoint.tolerance = 0.06f;
            springJoint.enableCollision = true;
        }

        public void CheckIfCanDropContent(Vector3 velocity)
        {
            //Debug.Log("m_boxesInCart.Count: " + m_boxesInCart.Count);
            //if (m_boxesInCart.Count <= 1) return;
            Debug.Log("CheckIfCanDropContent");


            Box1 box = _Tower.GetTopBox();

            if (box.IsEmpty() && _Tower.GetBoxesCount() > NB_UNDROPPABLE_BOXES)
                RemoveBoxImpulse(velocity);
            else if (!box.IsEmpty())
                box.RemoveItemImpulse(velocity);
        }
    }
}
