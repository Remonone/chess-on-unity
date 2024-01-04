using System;
using System.Collections.Generic;
using System.Linq;
using Chess.Pieces;
using Chess.Pieces.Data;
using Chess.Utils;
using Unity.Netcode;
using UnityEngine;

namespace Chess {
    using OccupationTable = Dictionary<Piece, List<Vector2Int>>;
    
    public class Board : NetworkBehaviour {
        

        [SerializeField] private float _cellSize;
        [SerializeField] private Vector2 _startingPoint;
        [SerializeField] private PieceBundle _bundle;

        private readonly Piece[,] _pieces = new Piece[8, 8];

        private Piece[,] _simulatedList = new Piece[8, 8];

        private OccupationTable _occupation = new();

        private PreviousStep _step;

        private bool _isKingChecked;

        public bool IsKingChecked => _isKingChecked;
        
        public Piece[,] Simulation => _simulatedList;
        public Piece[,] OriginalTable => _pieces;

        public bool IsCellOccupied(PlayerSide side, Vector2Int cell) => _occupation.Any(pair => pair.Value.Contains(cell) && pair.Key.ActiveSide != side);
        
        public PreviousStep GetPreviousStep() => _step;

        public Piece this[int x, int y] {
            get => _pieces[x, y];
            set => _pieces[x, y] = value;
        }

        public Piece this[Vector2Int position] {
            get => _pieces[position.x, position.y];
            set => _pieces[position.x, position.y] = value;
        }

        private void Start() {
            StartGame();
        }

        public Vector2Int GetBoardPositionByWorldPosition(Vector3 worldPosition) {
            var result = new Vector2Int((int)Math.Round((worldPosition.x - _startingPoint.x) / _cellSize - .5f), 
                (int)Math.Round(worldPosition.y / _cellSize - _startingPoint.y / _cellSize - .5f));
            return result;
        }

        public Vector3 GetWorldPositionByBoardPosition(Vector2Int position) {
            var worldPosition = new Vector3(_startingPoint.x + position.x * _cellSize + _cellSize / 2,
                _startingPoint.y + position.y * _cellSize + _cellSize / 2);
            return worldPosition;
        }

        public void StartGame() {
            SetPieceToMatrix("Rook", PlayerSide.WHITE, new Vector2Int(0, 0));
            SetPieceToMatrix("Knight", PlayerSide.WHITE, new Vector2Int(1, 0));
            SetPieceToMatrix("Bishop", PlayerSide.WHITE, new Vector2Int(2, 0));
            SetPieceToMatrix("King", PlayerSide.WHITE, new Vector2Int(3, 0));
            SetPieceToMatrix("Queen", PlayerSide.WHITE, new Vector2Int(4, 0));
            SetPieceToMatrix("Bishop", PlayerSide.WHITE, new Vector2Int(5, 0));
            SetPieceToMatrix("Knight", PlayerSide.WHITE, new Vector2Int(6, 0));
            SetPieceToMatrix("Rook", PlayerSide.WHITE, new Vector2Int(7, 0));

            for (int i = 0; i < 8; i++) SetPieceToMatrix("Pawn", PlayerSide.WHITE, new Vector2Int(i, 1));
            
            SetPieceToMatrix("Rook", PlayerSide.BLACK, new Vector2Int(0, 7));
            SetPieceToMatrix("Knight", PlayerSide.BLACK, new Vector2Int(1, 7));
            SetPieceToMatrix("Bishop", PlayerSide.BLACK, new Vector2Int(2, 7));
            SetPieceToMatrix("King", PlayerSide.BLACK, new Vector2Int(3, 7));
            SetPieceToMatrix("Queen", PlayerSide.BLACK, new Vector2Int(4, 7));
            SetPieceToMatrix("Bishop", PlayerSide.BLACK, new Vector2Int(5, 7));
            SetPieceToMatrix("Knight", PlayerSide.BLACK, new Vector2Int(6, 7));
            SetPieceToMatrix("Rook", PlayerSide.BLACK, new Vector2Int(7, 7));
            
            for (int i = 0; i < 8; i++) SetPieceToMatrix("Pawn", PlayerSide.BLACK, new Vector2Int(i, 6));
        }

        private void SetPieceToMatrix(string pieceName, PlayerSide side, Vector2Int position) {
            var prefab = _bundle.GetPieceByName(pieceName);
            var piecePosition = GetWorldPositionByBoardPosition(position);
            var piece = Instantiate(prefab, piecePosition, Quaternion.identity, transform);
            piece.Init(position, side);
            this[position] = piece;
        }

        public class PreviousStep {
            public Piece Piece;
            public Vector2Int PreviousPosition;
            public Vector2Int NewPosition;
        }

        public void MovePieceToNewPosition(Piece piece, PieceMove move) {
            this[piece.GetPosition()] = null;
            if (!ReferenceEquals(move.PieceUnderAttack, null)) {
                this[move.PieceUnderAttack.GetPosition()] = null;
                Destroy(move.PieceUnderAttack.gameObject);
            }
            this[move.Position] = piece;
            _step = new PreviousStep { Piece = piece, PreviousPosition = piece.GetPosition(), NewPosition = move.Position };
            piece.TranslatePosition(move.Position);
            UpdateOccupation();
        }
        
        
        private void UpdateOccupation() {
            var list = GetUpdatedOccupationList(_pieces, _step, true);
            _isKingChecked = CheckIsKingUnderAttack(list);
            _occupation = list;
        }

        private bool CheckIsKingUnderAttack(OccupationTable table) {
            var isKingUnderAttack = false;
            foreach (var piece in table) {
                foreach (var move in piece.Value) {
                    print(move + " " + this[move]);
                    if (!ReferenceEquals(this[move], null) 
                        && this[move].GetType() == typeof(King) 
                        && this[move].ActiveSide != piece.Key.ActiveSide) {
                        isKingUnderAttack = true;
                    }
                }
            }
            return isKingUnderAttack;
        }
        
        // TODO: Rework
        
        //IN PROGRESS
        private OccupationTable GetUpdatedOccupationList(Piece[,] pieceTable, PreviousStep prev, bool canSimulate) {
            if (_occupation.Keys.Count < 1) {
                var occupationList = FillOccupationList(pieceTable, canSimulate);
                return occupationList;
            }

            var pieces = new List<Piece>();
            var occupation = _occupation.ToDictionary(entry => entry.Key, entry => entry.Value);
            foreach (var direction in Directions.Complete) {
                for (int i = 0; i < 8; i++) {
                    var pos = direction * i + prev.PreviousPosition;
                    if(pos.x is < 0 or > 7 || pos.y is < 0 or > 7 || !this[pos]) continue;
                    var piece = this[pos];
                    occupation[piece].Clear();
                    pieces.Add(piece);
                    break;
                }
            }

            foreach (var piece in pieces) {
                var positions = piece.GetMovePositions(canSimulate);
                var moves = from move in positions
                    select move.Position;
                occupation[piece].AddRange(moves);
            }

            return occupation;
        }
        
        private OccupationTable FillOccupationList(Piece[,] pieceTable, bool canSimulate) {
            var occupations = new OccupationTable();
            for (int y = 0; y < 8; y++) {
                for (int x = 0; x < 8; x++) {
                    var piece = pieceTable[x, y];
                    if (ReferenceEquals(piece, null)) continue;
                    var pieceMoves = piece.GetMovePositions(canSimulate);
                    
                    var filteredMoves = from move in pieceMoves
                        select move.Position;
                    if (!occupations.ContainsKey(piece)) occupations[piece] = new List<Vector2Int>();
                    occupations[piece].AddRange(filteredMoves);
                }
            }
            return occupations;
        }

        public bool IsKingAttackedOnSimulate(Piece piece, PieceMove move) {
            var prev = new PreviousStep { Piece = piece, PreviousPosition = piece.GetPosition(), NewPosition = move.Position };
            SimulateMove(piece, move);
            var occupationList = GetUpdatedOccupationList(_simulatedList, prev, false);
            return CheckIsKingUnderAttack(occupationList);
        }

        private void SimulateMove(Piece piece, PieceMove move) {
            var pieces = _pieces.Clone() as Piece[,];
            var piecePosition = piece.GetPosition();
            pieces![piecePosition.x, piecePosition.y] = null;
            if (!ReferenceEquals(move.PieceUnderAttack, null)) {
                this[move.PieceUnderAttack.GetPosition()] = null;
            }
            pieces[move.Position.x, move.Position.y] = piece;
            _simulatedList = pieces;
        }
    }
}

