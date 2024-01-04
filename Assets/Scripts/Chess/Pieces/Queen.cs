using System.Collections.Generic;
using Chess.Utils;
using UnityEngine;

namespace Chess.Pieces {
    public class Queen : Piece {

        public override List<PieceMove> GetMovePositions(bool canSimulate) {
            return GetDirectedPositions(Directions.Complete, canSimulate);
        }
    }
}
