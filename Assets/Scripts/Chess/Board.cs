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

        private PreviousStep _step;

        public float CellSize => _cellSize;
        public PreviousStep GetPreviousStep() => _step;

        public Piece this[int x, int y] {
            get { return _pieces[x, y]; }
            set { _pieces[x, y] = value; }
        }

        public Piece this[Vector2Int position] {
            get { return _pieces[position.x, position.y]; }
            set { _pieces[position.x, position.y] = value; }
        }

        private void Start() {
            StartGame();
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
            var piecePosition = new Vector3(_startingPoint.x + position.x * _cellSize + _cellSize / 2,
                _startingPoint.y + position.y * _cellSize + _cellSize / 2);
            var piece = Instantiate(prefab, piecePosition, Quaternion.identity, transform);
            piece.Init(position, side);
            this[position] = piece;
        }

        public class PreviousStep {
            public Piece Piece;
            public Vector2Int PreviousPosition;
            public Vector2Int NewPosition;
        }
    }
}

