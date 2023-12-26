using System.Collections.Generic;
using UnityEngine;

namespace Chess.Pieces {
    public class Knight : Piece {
        
        private List<Vector2Int> _directions = new() { 
            new Vector2Int(1, 2), 
            new Vector2Int(-1,2), 
            new Vector2Int(-2,1), 
            new Vector2Int(-2, -1),
            new Vector2Int(2, 1), 
            new Vector2Int(2,-1), 
            new Vector2Int(-1,-2), 
            new Vector2Int(1, -2)
        };

        public override List<PieceMove> GetPositions() {
            List<PieceMove> positions = new();
            foreach (var direction in _directions) {
                var position = Position + direction;
                if(IsPointOutOfBound(position)) continue;
                if(!ReferenceEquals(Board[position], null) && Board[position].ActiveSide == ActiveSide) continue;
                positions.Add(new PieceMove{ Position = position, PieceUnderAttack = Board[position]});
            }
            return positions;
        }
    }
}
