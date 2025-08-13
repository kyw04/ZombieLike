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
        public bool isPlaced;
        public GameObject placedObj;
    }
    
    [RequireComponent(typeof(GridLayoutGroup))]
    public class Board : MonoBehaviour
    {
        public List<PiecePosition> piecePosition = new List<PiecePosition>();
        public float radius;

        private void FixedUpdate()
        {
            
        }

        private void OnDrawGizmosSelected()
        {
            foreach (var p in piecePosition)
            {
                if (Vector2.Distance(p.pos.position, p.piece.position) <= radius)
                    Gizmos.color = Color.green;
                else
                    Gizmos.color = Color.red;
                
                if (p.pos)
                    Gizmos.DrawWireSphere(p.pos.position, radius);
            }
        }
    }

}