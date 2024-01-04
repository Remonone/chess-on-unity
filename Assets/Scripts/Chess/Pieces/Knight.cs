using System.Collections.Generic;
using Chess.Utils;
using UnityEngine;

namespace Chess.Pieces {
    public class Knight : Piece {

        public override List<PieceMove> GetMovePositions(bool canSimulate) {
            var directions = Directions.Knight;
            var board = GetBoard(canSimulate);
            List<PieceMove> positions = new();
            if (Board.IsKingChecked) return positions;
            foreach (var direction in directions) {
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
