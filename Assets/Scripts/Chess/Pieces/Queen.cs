using System.Collections.Generic;
using Chess.Utils;

namespace Chess.Pieces {
    public class Queen : Piece {

        public override List<PieceMove> GetMovePositions(Table table, bool canSimulate) {
            return GetDirectedPositions(table, Directions.Complete, canSimulate);
        }
    }
}
