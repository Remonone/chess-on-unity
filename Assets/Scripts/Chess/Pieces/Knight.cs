using System.Collections.Generic;
using UnityEngine;

namespace Chess.Pieces {
    public class Knight : Piece {
        
        private readonly List<Vector2Int> _directions = new() { 
            new Vector2Int(1, 2), 
            new Vector2Int(-1,2), 
            new Vector2Int(-2,1), 
            new Vector2Int(-2, -1),
            new Vector2Int(2, 1), 
            new Vector2Int(2,-1), 
            new Vector2Int(-1,-2), 
            new Vector2Int(1, -2)
        };

        public override List<PieceMove> GetMovePositions(bool canSimulate) {
            var board = GetBoard(canSimulate);
            List<PieceMove> positions = new();
            if (Board.IsKingChecked) return positions;
            foreach (var direction in _directions) {
                var position = Position + direction;
                if(IsPointOutOfBound(position)) continue;
                if(board[position.x, position.y] && board[position.x, position.y].ActiveSide == ActiveSide) continue;
                var move = new PieceMove { Position = position, PieceUnderAttack = board[position.x, position.y] };
                if(canSimulate && Board.IsKingChecked && Board.IsKingAttackedOnSimulate(this, move)) continue;
                positions.Add(move);
            }
            return positions;
        }
    }
}
