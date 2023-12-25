using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chess.Pieces {
    public class Pawn : Piece {

        public override List<PieceMove> GetPositions() {
            List<PieceMove> positions = new();
            if (ActiveSide == PlayerSide.WHITE) {
                if (Board[Position.x + 1, Position.y + 1] != null) {
                    var position = new Vector2Int(Position.x + 1, Position.y + 1);
                    positions.Add(new PieceMove {Position = position, PieceUnderAttack = Board[position]});
                }
                if(Board[Position.x - 1, Position.y + 1] != null) {
                    var position = new Vector2Int(Position.x - 1, Position.y + 1);
                    positions.Add(new PieceMove{Position = position, PieceUnderAttack = Board[position]});
                }
                if (Board[Position.x, Position.y + 1] != null) return positions;
                positions.Add(new PieceMove { Position = new Vector2Int(Position.x, Position.y + 1) });
                if (Position.y == 2 && Board[Position.x, Position.y + 2] == null) 
                    positions.Add(new PieceMove { Position = new Vector2Int(Position.x, Position.y + 2) });
                var previousStep = Board.GetPreviousStep();
                if (Position.y != 5
                    || (previousStep.NewPosition.y - previousStep.PreviousPosition.y) != 2
                    || Math.Abs(previousStep.NewPosition.x - Position.x) != 1) return positions;
                var enPassant = new Vector2Int(previousStep.NewPosition.x, 6);
                positions.Add(new PieceMove{ Position = enPassant, PieceUnderAttack = previousStep.Piece });

                return positions;
            } else {
                if (Board[Position.x + 1, Position.y + 1] != null) {
                    var position = new Vector2Int(Position.x + 1, Position.y + 1);
                    positions.Add(new PieceMove {Position = position, PieceUnderAttack = Board[position]});
                }
                if(Board[Position.x - 1, Position.y + 1] != null) {
                    var position = new Vector2Int(Position.x - 1, Position.y + 1);
                    positions.Add(new PieceMove{Position = position, PieceUnderAttack = Board[position]});
                }
                if (Board[Position.x, Position.y + 1] != null) return positions;
                positions.Add(new PieceMove { Position = new Vector2Int(Position.x, Position.y + 1) });
                if (Position.y == 2 && Board[Position.x, Position.y + 2] == null) 
                    positions.Add(new PieceMove { Position = new Vector2Int(Position.x, Position.y + 2) });
                var previousStep = Board.GetPreviousStep();
                if (Position.y != 4
                    || (previousStep.NewPosition.y - previousStep.PreviousPosition.y) != 2
                    || Math.Abs(previousStep.NewPosition.x - Position.x) != 1) return positions;
                var enPassant = new Vector2Int(previousStep.NewPosition.x, 5);
                positions.Add(new PieceMove{ Position = enPassant, PieceUnderAttack = previousStep.Piece });

                return positions;
            }
        }
    }
}
