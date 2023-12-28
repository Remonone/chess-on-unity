using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chess.Pieces {
    public class Pawn : Piece {

        private static readonly List<PawnConfig> Configs = new() {
            new PawnConfig { Side = PlayerSide.WHITE, StartPosition = 1, VerticalDirection = 1, EnPassantPosition = 4 },
            new PawnConfig { Side = PlayerSide.BLACK, StartPosition = 6, VerticalDirection = -1, EnPassantPosition = 3 }
        };

        public override List<PieceMove> GetMovePositions(bool canSimulate) {
            var board = GetBoard(canSimulate);
            List<PieceMove> positions = new();
            var config = Configs.First(c => c.Side == ActiveSide);

            for (int i = -1; i < 2; i += 2) {
                var checkPosition = new Vector2Int(Position.x + i, Position.y + config.VerticalDirection);
                if (!IsPointOutOfBound(checkPosition) && board[checkPosition.x, checkPosition.y] && board[checkPosition.x, checkPosition.y].ActiveSide != ActiveSide) {
                    var pieceMove = new PieceMove {Position = checkPosition, PieceUnderAttack = board[checkPosition.x, checkPosition.y]};
                    if (!(canSimulate && Board.IsKingChecked && Board.IsKingAttackedOnSimulate(this, pieceMove)))
                        positions.Add(pieceMove);
                }
            }

            var previousStep = Board.GetPreviousStep();
            if (Position.y == config.EnPassantPosition && Math.Abs(previousStep.NewPosition.y - previousStep.PreviousPosition.y) == 2 && Math.Abs(previousStep.NewPosition.x - Position.x) == 1) {
                var enPassant = new Vector2Int(previousStep.NewPosition.x, config.EnPassantPosition + config.VerticalDirection);
                positions.Add(new PieceMove{ Position = enPassant, PieceUnderAttack = previousStep.Piece});
            }
            
            if (board[Position.x, Position.y + config.VerticalDirection]) return positions;
            var move = new PieceMove { Position = new Vector2Int(Position.x, Position.y + config.VerticalDirection) };
            if (!(canSimulate && Board.IsKingChecked && Board.IsKingAttackedOnSimulate(this, move))) 
                positions.Add(move);
            if (Position.y == config.StartPosition && ReferenceEquals(board[Position.x, Position.y + config.VerticalDirection * 2], null)) {
                move = new PieceMove
                    { Position = new Vector2Int(Position.x, Position.y + config.VerticalDirection * 2) };
                if(!(canSimulate && Board.IsKingChecked && Board.IsKingAttackedOnSimulate(this, move)))
                    positions.Add(move);
            }
                
            
            return positions;
        }

        private sealed class PawnConfig {
            public PlayerSide Side;
            public int VerticalDirection;
            public int StartPosition;
            public int EnPassantPosition;
        }
    }
}
