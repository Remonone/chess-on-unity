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

        public override List<PieceMove> GetPositions(Vector2Int currentPosition) {
            List<PieceMove> positions = new();
            foreach (var direction in _directions) {
                var position = currentPosition + direction;
                if(IsPointOutOfBound(position)) continue;
                if(Board[position].ActiveSide == ActiveSide) continue;
                var newPosition = direction + currentPosition;
                positions.Add(new PieceMove{ Position = newPosition, PieceUnderAttack = Board[newPosition]});
            }
            return positions;
        }
    }
}
