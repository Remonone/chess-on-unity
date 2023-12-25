using System.Collections.Generic;
using UnityEngine;

namespace Chess.Pieces {
    public class King : Piece {
        private List<Vector2Int> _directions = new() { 
            new Vector2Int(1, 1), 
            new Vector2Int(1,-1), 
            new Vector2Int(-1,1), 
            new Vector2Int(-1, -1),
            new Vector2Int(0, 1), 
            new Vector2Int(1,0), 
            new Vector2Int(-1,0), 
            new Vector2Int(0, -1)
        };

        public override List<PieceMove> GetPositions() {
            List<PieceMove> positions = new();
            foreach (var direction in _directions) {
                var position = Position + direction;
                if(IsPointOutOfBound(position)) continue;
                if(Board[position.x, position.y].ActiveSide == ActiveSide) continue;
                // TODO: Check if point is under attack
                positions.Add(new PieceMove { Position = position, PieceUnderAttack = Board[position]});
            }
            return positions;
        }
    }
}
