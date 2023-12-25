using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chess.Pieces.Data {
    [CreateAssetMenu(fileName = "Pieces", menuName = "Chess/Create Piece Bundle", order = 0)]
    public class PieceBundle : ScriptableObject {

        [SerializeField] private List<PieceConfig> _configs;

        public Piece GetPieceByName(string pieceName) => _configs.Find(piece => piece.Name == pieceName).Piece;
        
        [Serializable]
        internal class PieceConfig {
            public string Name;
            public Piece Piece;
        }
    }
}
