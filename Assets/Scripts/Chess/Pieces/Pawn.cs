using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chess.Pieces {
    public class Pawn : Piece {

        public override List<PieceMove> GetPositions(Vector2Int currentPosition) {
            List<PieceMove> positions = new();
            if (ActiveSide == PlayerSide.WHITE) {
                if (Board[currentPosition.x + 1, currentPosition.y + 1] != null) {
                    var position = new Vector2Int(currentPosition.x + 1, currentPosition.y + 1);
                    positions.Add(new PieceMove {Position = position, PieceUnderAttack = Board[position]});
                }
                if(Board[currentPosition.x - 1, currentPosition.y + 1] != null) {
                    var position = new Vector2Int(currentPosition.x - 1, currentPosition.y + 1);
                    positions.Add(new PieceMove{Position = position, PieceUnderAttack = Board[position]});
                }
                if (Board[currentPosition.x, currentPosition.y + 1] != null) return positions;
                positions.Add(new PieceMove { Position = new Vector2Int(currentPosition.x, currentPosition.y + 1) });
                if (currentPosition.y == 2 && Board[currentPosition.x, currentPosition.y + 2] == null) 
                    positions.Add(new PieceMove { Position = new Vector2Int(currentPosition.x, currentPosition.y + 2) });
                var previousStep = Board.GetPreviousStep();
                if (currentPosition.y != 5
                    || (previousStep.NewPosition.y - previousStep.PreviousPosition.y) != 2
                    || Math.Abs(previousStep.NewPosition.x - currentPosition.x) != 1) return positions;
                var enPassant = new Vector2Int(previousStep.NewPosition.x, 6);
                positions.Add(new PieceMove{ Position = enPassant, PieceUnderAttack = previousStep.Piece });

                return positions;
            } else {
                if (Board[currentPosition.x + 1, currentPosition.y + 1] != null) {
                    var position = new Vector2Int(currentPosition.x + 1, currentPosition.y + 1);
                    positions.Add(new PieceMove {Position = position, PieceUnderAttack = Board[position]});
                }
                if(Board[currentPosition.x - 1, currentPosition.y + 1] != null) {
                    var position = new Vector2Int(currentPosition.x - 1, currentPosition.y + 1);
                    positions.Add(new PieceMove{Position = position, PieceUnderAttack = Board[position]});
                }
                if (Board[currentPosition.x, currentPosition.y + 1] != null) return positions;
                positions.Add(new PieceMove { Position = new Vector2Int(currentPosition.x, currentPosition.y + 1) });
                if (currentPosition.y == 2 && Board[currentPosition.x, currentPosition.y + 2] == null) 
                    positions.Add(new PieceMove { Position = new Vector2Int(currentPosition.x, currentPosition.y + 2) });
                var previousStep = Board.GetPreviousStep();
                if (currentPosition.y != 5
                    || (previousStep.NewPosition.y - previousStep.PreviousPosition.y) != 2
                    || Math.Abs(previousStep.NewPosition.x - currentPosition.x) != 1) return positions;
                var enPassant = new Vector2Int(previousStep.NewPosition.x, 6);
                positions.Add(new PieceMove{ Position = enPassant, PieceUnderAttack = previousStep.Piece });

                return positions;
            }
        }
    }
}
