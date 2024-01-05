using System;
using Chess.Pieces;
using Chess.Pieces.Data;
using Unity.Netcode;
using UnityEngine;

namespace Chess {
    
    public class Board : NetworkBehaviour {
        
        [SerializeField] private float _cellSize;
        [SerializeField] private Vector2 _startingPoint;
        [SerializeField] private PieceBundle _bundle;

        private Table _table;
        private Table _simulate;

        public Table Table => _table;

        private void Awake() {
            _table = new Table();
            _simulate = new Table();
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

            _table.Init();
        }

        private void SetPieceToMatrix(string pieceName, PlayerSide side, Vector2Int position) {
            var prefab = _bundle.GetPieceByName(pieceName);
            var piecePosition = GetWorldPositionByBoardPosition(position);
            var piece = Instantiate(prefab, piecePosition, Quaternion.identity, transform);
            piece.Init(position, side);
            _table.SetPieceToTable(position, piece.Info);
        }

        public void MovePieceToNewPosition(Piece piece, PieceMove move) {
            
            if (!ReferenceEquals(move.PieceUnderAttack, null)) {
                var pieceToDestroy = move.PieceUnderAttack;
                _table.DeletePiece(pieceToDestroy.Position);
                Destroy(pieceToDestroy.gameObject);
            }
            _table.TransferPiece(piece.Position, move.Position);
            piece.TranslatePosition(move.Position);
            // TODO: OPTIMIZE COMPLEXITY
            _table.UpdateOccupationInfo(false);
            _table.UpdateOccupationInfo(true);
            
        }

        public bool IsKingAttackedOnSimulate(Piece piece, PieceMove move) {
            SimulateMove(piece, move);
            _simulate.UpdateOccupationInfo(false);
            return _simulate.IsKingChecked;
        }

        private void SimulateMove(Piece piece, PieceMove move) {
            _table.CopyTo(_simulate);
            if (!ReferenceEquals(move.PieceUnderAttack, null)) {
                _simulate.DeletePiece(move.PieceUnderAttack.Position);
            }
            _simulate.TransferPiece(piece.Position, move.Position);
        }

        
    }
}

