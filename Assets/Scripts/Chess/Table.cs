using System.Collections.Generic;
using System.Linq;
using Chess.Pieces;
using UnityEngine;

namespace Chess {
    using OccupationTable = Dictionary<PieceInfo, List<PieceMove>>;
    
    public class Table {
        
        private PieceInfo[,] _pieces = new PieceInfo[8, 8];
        private OccupationTable _occupationTable = new();
        private bool _isKingChecked;

        public bool IsKingChecked => _isKingChecked;
        public OccupationTable Occupation => _occupationTable;
        
        private PreviousStep _previousStep;
        public PreviousStep GetPreviousStep() => _previousStep;
        
        public PieceInfo this[int x, int y] {
            get => _pieces[x, y];
            private set => _pieces[x, y] = value;
        }

        public PieceInfo this[Vector2Int position] {
            get => _pieces[position.x, position.y];
            private set => _pieces[position.x, position.y] = value;
        }
        

        public void CopyTo(Table copyTo) {
            copyTo._pieces = CloneTable();
            copyTo._occupationTable = CloneOccupation();
            copyTo._isKingChecked = _isKingChecked;
        }
        
        public void UpdateOccupationInfo(bool canSimulate) {
            UpdateOccupationList(canSimulate);
            _isKingChecked = IsKingUnderAttack();
        }

        public void SetPieceToTable(Vector2Int position, PieceInfo piece) {
            this[position] = piece;
        }

        public void Init() {
            _occupationTable = FillOccupationList(_pieces, true);
        }

        public void DeletePiece(Vector2Int position) {
            _occupationTable.Remove(this[position]);
            this[position] = null;
        }

        public void TransferPiece(Vector2Int origin, Vector2Int destination) {
            var pieceInfo = this[origin];
            if (pieceInfo == null) return;
            this[origin] = null;
            this[destination] = pieceInfo;
            _previousStep = new PreviousStep
                { Piece = pieceInfo, PreviousPosition = origin, NewPosition = destination };
            _occupationTable[pieceInfo] = pieceInfo.Reference.GetMovePositions(this, false);
        }

        public void UpdateOccupationList(bool canSimulate) {
            var occupations = new OccupationTable();
            for (int y = 0; y < 8; y++) {
                for (int x = 0; x < 8; x++) {
                    var piece = this[x, y];
                    if (ReferenceEquals(piece, null)) continue;
                    var pieceMoves = piece.Reference.GetMovePositions(this, canSimulate);
                    occupations[piece] = new List<PieceMove>();
                    occupations[piece].AddRange(pieceMoves);
                }
            }
            _occupationTable = occupations;
        }
        
        private bool IsKingUnderAttack() {
            var isKingUnderAttack = false;
            foreach (var pair in _occupationTable) {
                foreach (var move in pair.Value) {
                    var position = move.Position;
                    if (!ReferenceEquals(this[position.x,position.y], null) 
                        && this[position.x,position.y].Reference.GetType() == typeof(King) 
                        && this[position.x,position.y].Side != pair.Key.Side) {
                        isKingUnderAttack = true;
                    }
                }
            }
            return isKingUnderAttack;
        }
        
        private OccupationTable FillOccupationList(PieceInfo[,] pieceInfoTable, bool canSimulate) {
            var occupations = _occupationTable.Count < 1 ? new OccupationTable() : _occupationTable.ToDictionary(entry => entry.Key, entry => entry.Value);
            for (int y = 0; y < 8; y++) {
                for (int x = 0; x < 8; x++) {
                    var piece = pieceInfoTable[x, y];
                    if (ReferenceEquals(piece, null)) continue;
                    var pieceMoves = piece.Reference.GetMovePositions(this, canSimulate);
                    occupations[piece] = new List<PieceMove>();
                    occupations[piece].AddRange(pieceMoves);
                }
            }
            return occupations;
        }
        
        private PieceInfo[,] CloneTable() {
            PieceInfo[,] info = new PieceInfo[8, 8];
            for (int x = 0; x < 8; x++) {
                for (int y = 0; y < 8; y++) {
                    info[x, y] = this[x, y]?.Clone();
                }
            }

            return info;
        }

        private OccupationTable CloneOccupation() =>
            _occupationTable.ToDictionary(entry => entry.Key, entry => entry.Value);

        public class PreviousStep {
            public PieceInfo Piece;
            public Vector2Int PreviousPosition;
            public Vector2Int NewPosition;
        }
    }
}
