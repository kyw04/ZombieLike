using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Puzzle
{
    [Serializable] public class PiecePosition
    {
        public Transform pos;
        public Transform piece;
    }
    
    [RequireComponent(typeof(GridLayoutGroup))]
    public class Board : MonoBehaviour
    {
        [SerializeField] private List<PiecePosition> piecePosition = new List<PiecePosition>();
        [SerializeField] private float radius;
        private void Awake()
        {
            
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            foreach (var p in piecePosition)
            {
                if (p.pos)
                    Gizmos.DrawWireSphere(p.pos.position, radius);
            }
        }
    }

}