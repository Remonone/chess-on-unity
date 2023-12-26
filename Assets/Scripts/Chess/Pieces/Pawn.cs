using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chess.Pieces {
    public class Pawn : Piece {

        public override List<PieceMove> GetMovePositions() {
            List<PieceMove> positions = new();
            Vector2Int checkPosition;
            if (ActiveSide == PlayerSide.WHITE) {
                checkPosition = new Vector2Int(Position.x + 1, Position.y + 1);
                if (!IsPointOutOfBound(checkPosition) 
                    && !ReferenceEquals(Board[checkPosition], null)
                    && Board[checkPosition].ActiveSide != ActiveSide) {
                    positions.Add(new PieceMove {Position = checkPosition, PieceUnderAttack = Board[checkPosition]});
                }
                checkPosition = new Vector2Int(Position.x - 1, Position.y + 1);
                if(!IsPointOutOfBound(checkPosition) 
                   && !ReferenceEquals(Board[checkPosition], null)
                   && Board[checkPosition].ActiveSide != ActiveSide) {
                    positions.Add(new PieceMove{Position = checkPosition, PieceUnderAttack = Board[checkPosition]});
                }
                if (!ReferenceEquals(Board[Position.x, Position.y + 1], null)) return positions;
                positions.Add(new PieceMove { Position = new Vector2Int(Position.x, Position.y + 1) });
                if (Position.y == 1 && ReferenceEquals(Board[Position.x, Position.y + 2], null)) 
                    positions.Add(new PieceMove { Position = new Vector2Int(Position.x, Position.y + 2) });
                var previousStep = Board.GetPreviousStep();
                if (Position.y != 5
                    || (previousStep.NewPosition.y - previousStep.PreviousPosition.y) != 2
                    || Math.Abs(previousStep.NewPosition.x - Position.x) != 1) return positions;
                var enPassant = new Vector2Int(previousStep.NewPosition.x, 6);
                positions.Add(new PieceMove{ Position = enPassant, PieceUnderAttack = previousStep.Piece });

                return positions;
            } else {
                checkPosition = new Vector2Int(Position.x + 1, Position.y - 1);
                if (!IsPointOutOfBound(checkPosition) 
                    && !ReferenceEquals(Board[checkPosition], null)
                    && Board[checkPosition].ActiveSide != ActiveSide) {
                    positions.Add(new PieceMove {Position = checkPosition, PieceUnderAttack = Board[checkPosition]});
                }
                checkPosition = new Vector2Int(Position.x - 1, Position.y - 1);
                if(!IsPointOutOfBound(checkPosition) 
                   && !ReferenceEquals(Board[checkPosition], null)
                   && Board[checkPosition].ActiveSide != ActiveSide) {
                    positions.Add(new PieceMove{Position = checkPosition, PieceUnderAttack = Board[checkPosition]});
                }
                if (!ReferenceEquals(Board[Position.x, Position.y - 1], null)) return positions;
                positions.Add(new PieceMove { Position = new Vector2Int(Position.x, Position.y - 1) });
                if (Position.y == 1 && ReferenceEquals(Board[Position.x, Position.y - 2], null)) 
                    positions.Add(new PieceMove { Position = new Vector2Int(Position.x, Position.y - 2) });
                var previousStep = Board.GetPreviousStep();
                if (Position.y != 4
                    || (previousStep.NewPosition.y - previousStep.PreviousPosition.y) != 2
                    || Math.Abs(previousStep.NewPosition.x - Position.x) != 1) return positions;
                var enPassant = new Vector2Int(previousStep.NewPosition.x, 3);
                positions.Add(new PieceMove{ Position = enPassant, PieceUnderAttack = previousStep.Piece });

                return positions;
            }
        }
    }
}
