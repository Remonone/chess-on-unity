using System;
using System.Collections.Generic;
using System.Linq;
using Chess.Pieces;
using Chess.Pieces.Data;
using Unity.Netcode;
using UnityEngine;

namespace Chess {
    public class Board : NetworkBehaviour {

        [SerializeField] private float _cellSize;
        [SerializeField] private Vector2 _startingPoint;
        [SerializeField] private PieceBundle _bundle;

        private readonly Piece[,] _pieces = new Piece[8, 8];

        private readonly Dictionary<PlayerSide, List<Vector2Int>> _occupationDictionary = new();

        private PreviousStep _step;

        private bool _isKingChecked;

        private Piece _threat;

        //TODO: Hunting the threat;
        public Piece Threat => _threat;
        public bool IsKingChecked => _isKingChecked;

        public bool IsCellOccupied(PlayerSide side, Vector2Int cell) => _occupationDictionary[side].Contains(cell);
        
        public PreviousStep GetPreviousStep() => _step;

        public Piece this[int x, int y] {
            get => _pieces[x, y];
            set => _pieces[x, y] = value;
        }

        public Piece this[Vector2Int position] {
            get => _pieces[position.x, position.y];
            set => _pieces[position.x, position.y] = value;
        }

        private void Awake() {
            _occupationDictionary[PlayerSide.BLACK] = new List<Vector2Int>();
            _occupationDictionary[PlayerSide.WHITE] = new List<Vector2Int>();
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
            UpdateOccupationList();
        }
        
        
        private void UpdateOccupationList() {
            var isKingUnderAttack = false;
            for (int y = 0; y < 8; y++) {
                for (int x = 0; x < 8; x++) {
                    var piece = this[x, y];
                    if(ReferenceEquals(piece, null)) continue;
                    var pieceMoves = piece.GetMovePositions();
                    var king = pieceMoves?
                        .FirstOrDefault(move => move.PieceUnderAttack && 
                                                move.PieceUnderAttack.GetType() == typeof(King) && 
                                                piece.ActiveSide != move.PieceUnderAttack.ActiveSide
                                                && move.IsReachable);
                    if (king != null) {
                        _threat = piece;
                        isKingUnderAttack = true;
                    }
                    
                    var filteredMoves = from move in pieceMoves
                        where !_occupationDictionary[piece.ActiveSide].Contains(move.Position) && move.IsReachable
                        select move.Position;
                    foreach(var move in filteredMoves) _occupationDictionary[piece.ActiveSide].Add(move);
                }
            }
            _isKingChecked = isKingUnderAttack;
        }
        
    }
}

