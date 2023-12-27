using System.Collections.Generic;
using Chess.Utils;
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

        public override List<PieceMove> GetMovePositions() {
            List<PieceMove> positions = new();
            foreach (var direction in _directions) {
                var position = Position + direction;
                if(IsPointOutOfBound(position)) continue;
                if(!ReferenceEquals(Board[position], null) && Board[position].ActiveSide == ActiveSide) continue;
                if(Board.IsCellOccupied(SideSwap.InvertSide(ActiveSide), position)) continue;
                positions.Add(new PieceMove { Position = position, PieceUnderAttack = Board[position], IsReachable = true});
            }
            return positions;
        }
    }
}
