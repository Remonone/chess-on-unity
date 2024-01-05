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

        public override List<PieceMove> GetMovePositions(Table table, bool canSimulate) {
            List<PieceMove> positions = new();
            var config = Configs.First(c => c.Side == Info.Side);
            var position = Info.Position;

            for (int i = -1; i < 2; i += 2) {
                var checkPosition = new Vector2Int(position.x + i, position.y + config.VerticalDirection);
                if (!IsPointOutOfBound(checkPosition) && !ReferenceEquals(table[checkPosition.x, checkPosition.y], null) && table[checkPosition.x, checkPosition.y].Side != Info.Side) {
                    var pieceMove = new PieceMove {Position = checkPosition, PieceUnderAttack = table[checkPosition.x, checkPosition.y].Reference};
                    if (!(canSimulate && _info.Board.IsKingAttackedOnSimulate(this, pieceMove)))
                        positions.Add(pieceMove);
                }
            }

            var previousStep = table.GetPreviousStep();
            if (position.y == config.EnPassantPosition && Math.Abs(previousStep.NewPosition.y - previousStep.PreviousPosition.y) == 2 && Math.Abs(previousStep.NewPosition.x - position.x) == 1) {
                var enPassant = new PieceMove {
                    Position = new Vector2Int(previousStep.NewPosition.x, config.EnPassantPosition + config.VerticalDirection), 
                    PieceUnderAttack = previousStep.Piece.Reference
                };
                if (!(canSimulate && Board.IsKingAttackedOnSimulate(this, enPassant)))
                    positions.Add(enPassant);
            }
            
            if (table[position.x, position.y + config.VerticalDirection]?.Reference) return positions;
            var move = new PieceMove { Position = new Vector2Int(position.x, position.y + config.VerticalDirection) };
            if (!(canSimulate && _info.Board.IsKingAttackedOnSimulate(this, move))) 
                positions.Add(move);
            if (position.y == config.StartPosition && ReferenceEquals(table[position.x, position.y + config.VerticalDirection * 2], null)) {
                move = new PieceMove
                    { Position = new Vector2Int(position.x, position.y + config.VerticalDirection * 2) };
                if(!(canSimulate && Board.IsKingAttackedOnSimulate(this, move)))
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
