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
    
    public class Board : MonoBehaviour
    {
        public List<PiecePosition> piecePosition = new List<PiecePosition>();
        public float radius;
        private int correctCount;
        
        private void FixedUpdate()
        {
            correctCount = 0;
            foreach (var p in piecePosition)
            {
                if (Vector2.Distance(p.pos.position, p.piece.position) <= 5f)
                {
                    correctCount++;
                }
            }
            
            if (correctCount == piecePosition.Count)
                Debug.Log("전부 정확한 위치에 들어갔습니다!");
        }

        private void OnDrawGizmosSelected()
        {
            foreach (var p in piecePosition)
            {
                if (Vector2.Distance(p.pos.position, p.piece.position) <= 5f)
                    Gizmos.color = Color.green;
                else
                    Gizmos.color = Color.red;
                
                if (p.pos)
                    Gizmos.DrawWireSphere(p.pos.position, radius);
            }
        }
    }

}