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

        public override List<PieceMove> GetMovePositions() {
            List<PieceMove> positions = new();
            if (Board.IsKingChecked) return positions;
            foreach (var direction in _directions) {
                var position = Position + direction;
                if(IsPointOutOfBound(position)) continue;
                positions.Add(new PieceMove{ Position = position, 
                    PieceUnderAttack = Board[position], 
                    IsReachable = ReferenceEquals(Board[position], null) || Board[position].ActiveSide != ActiveSide});
            }
            return positions;
        }
    }
}
