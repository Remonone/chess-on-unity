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

        public override List<PieceMove> GetMovePositions(bool canSimulate) {
            var board = GetBoard(canSimulate);
            List<PieceMove> positions = new();
            bool[] isDirectionReachesEnd = new bool[_directions.Count];
            for (int i = 1; i < 8; i++) {
                for (int j = 0; j < _directions.Count; j++) {
                    if (isDirectionReachesEnd[j]) continue;
                    Vector2Int newPosition = _directions[j] * i + Position;
                    if (IsPointOutOfBound(newPosition)) {
                        isDirectionReachesEnd[j] = true;
                        continue;
                    }
                    if (ReferenceEquals(board[newPosition.x, newPosition.y], null)) {
                        var move = new PieceMove { Position = newPosition };
                        if(canSimulate && Board.IsKingChecked && Board.IsKingAttackedOnSimulate(this, move)) continue;
                        positions.Add(move);
                        continue;
                    }

                    if (board[newPosition.x, newPosition.y].ActiveSide != ActiveSide) {
                        var move = new PieceMove { Position = newPosition, PieceUnderAttack = board[newPosition.x, newPosition.y] };
                        if(canSimulate && Board.IsKingChecked && Board.IsKingAttackedOnSimulate(this, move)) continue;
                        positions.Add(move);
                    }
                    isDirectionReachesEnd[j] = true;
                }
            }

            return positions;
        }
    }
}
