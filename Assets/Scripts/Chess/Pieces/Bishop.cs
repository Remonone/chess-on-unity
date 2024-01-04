using System.Collections.Generic;
using Chess.Utils;
using UnityEngine;

namespace Chess.Pieces {
    public class Bishop : Piece {
        
        public override List<PieceMove> GetMovePositions(bool canSimulate) {
            return GetDirectedPositions(Directions.Diagonal, canSimulate);
        }
        
    }
}
