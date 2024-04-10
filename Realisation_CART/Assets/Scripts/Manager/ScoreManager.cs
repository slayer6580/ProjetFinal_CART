using System.Collections.Generic;
using UnityEngine;
using BoxSystem;
using Box = BoxSystem.Box;

namespace Manager
{
    public static class ScoreManager
    {
        public static TowerBoxSystem _TowerBoxSystem { get; set; }

        [SerializeField] private static int m_cartokenValueMultiplier = 1;
        

        /// <summary> Vide le panier et rend le nombre d'items de la tour et le score total </summary>
        public static Vector3 EmptyCartAndGetScore()
        {
            CalculateCartokens();

            int totalScore = 0;
            int nbOfItems = 0;
            int nbOfCartokens = CalculateCartokens();

            while (_TowerBoxSystem.GetBoxCount() > 0)
            {
                Box topBox = _TowerBoxSystem.GetTopBox();
                List<Box.ItemInBox> itemsInBox = topBox.GetItemsInBox();

                for (int i = 0; i < itemsInBox.Count; i++)
                {
                    nbOfItems++;
                    totalScore += itemsInBox[i].m_item.GetComponent<Item>().m_data.m_cost;
                }
                _TowerBoxSystem.RemoveBoxImpulse();
            }

            return new Vector3(nbOfItems, totalScore, nbOfCartokens);
        }

        private static int CalculateCartokens()
        {
            return _TowerBoxSystem.GetBoxCount() * m_cartokenValueMultiplier;
        }
    }
}
