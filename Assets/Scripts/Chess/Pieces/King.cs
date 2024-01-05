using System.Collections.Generic;
using Chess.Utils;

namespace Chess.Pieces {
    public class King : Piece {

        public override List<PieceMove> GetMovePositions(Table table, bool canSimulate) {
            List<PieceMove> positions = new();
            foreach (var direction in Directions.Complete) {
                var position = Position + direction;
                if(IsPointOutOfBound(position)) continue;
                if(!ReferenceEquals(table[position.x, position.y], null) && table[position.x, position.y].Side == Side) continue;
                var move = new PieceMove
                    { Position = position, PieceUnderAttack = table[position.x, position.y]?.Reference };
                if(canSimulate && Board.IsKingAttackedOnSimulate(this, move)) continue;
                positions.Add(move);
            }
            return positions;
        }
    }
}
