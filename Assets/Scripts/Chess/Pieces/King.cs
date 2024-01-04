using System.Collections.Generic;
using Chess.Utils;
using UnityEngine;

namespace Chess.Pieces {
    public class King : Piece {

        public override List<PieceMove> GetMovePositions(bool canSimulate) {
            var board = GetBoard(canSimulate);
            List<PieceMove> positions = new();
            foreach (var direction in Directions.Complete) {
                var position = Position + direction;
                if(IsPointOutOfBound(position)) continue;
                if(!ReferenceEquals(board[position.x, position.y], null) && board[position.x, position.y].ActiveSide == ActiveSide) continue;
                if(Board.IsCellOccupied(ActiveSide, position)) continue;
                positions.Add(new PieceMove { Position = position, PieceUnderAttack = board[position.x, position.y]});
            }
            return positions;
        }
    }
}
