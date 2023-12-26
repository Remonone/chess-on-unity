using System.Collections.Generic;
using UnityEngine;

namespace Chess.Pieces {
    public class Queen : Piece {
        
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
            bool[] isDirectionReachesEnd = new bool[_directions.Count];
            for (int i = 1; i < 8; i++) {
                for (int j = 0; j < _directions.Count; j++) {
                    if(isDirectionReachesEnd[j]) continue;
                    Vector2Int newPosition = _directions[j] * i + Position;
                    if (IsPointOutOfBound(newPosition)) {
                        isDirectionReachesEnd[j] = true;
                        continue;
                    }
                    if (Board[newPosition] != null) {
                        isDirectionReachesEnd[j] = true;
                        if(Board[newPosition].ActiveSide == ActiveSide) continue;
                    }
                    positions.Add(new PieceMove{ Position = newPosition, PieceUnderAttack = Board[newPosition]});
                }
            }

            return positions;
        }
    }
}
