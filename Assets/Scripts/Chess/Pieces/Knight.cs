using System.Collections.Generic;
using Chess.Utils;

namespace Chess.Pieces {
    public class Knight : Piece {

        public override List<PieceMove> GetMovePositions(Table table, bool canSimulate) {
            var directions = Directions.Knight;
            List<PieceMove> positions = new();
            foreach (var direction in directions) {
                var position = Info.Position + direction;
                if(IsPointOutOfBound(position)) continue;
                if(!ReferenceEquals(table[position.x, position.y], null) && table[position.x, position.y].Side == Info.Side) continue;
                var move = new PieceMove { Position = position, PieceUnderAttack = table[position.x, position.y]?.Reference };
                if(canSimulate && Info.Board.IsKingAttackedOnSimulate(this, move)) continue;
                positions.Add(move);
            }
            return positions;
        }
    }
}
