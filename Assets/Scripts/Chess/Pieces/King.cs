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

        public override List<PieceMove> GetPositions(Vector2Int currentPosition) {
            List<PieceMove> positions = new();
            foreach (var direction in _directions) {
                var position = currentPosition + direction;
                if(IsPointOutOfBound(position)) continue;
                if(Board[position.x, position.y].ActiveSide == ActiveSide) continue;
                // TODO: Check if point is under attack
                var newPosition = direction + currentPosition;
                positions.Add(new PieceMove { Position = newPosition, PieceUnderAttack = Board[newPosition]});
            }
            return positions;
        }
    }
}
