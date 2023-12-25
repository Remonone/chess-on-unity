using Chess.Pieces;
using Unity.Netcode;
using UnityEngine;

namespace Chess {
    public class Board : NetworkBehaviour {

        [SerializeField] private float _cellSize;
        [SerializeField] private Vector2 _startingPoint;

        private Piece[,] _pieces = new Piece[8, 8];

        private PreviousStep _step;

        public PreviousStep GetPreviousStep() => _step;

        public Piece this[int x, int y] {
            get { return _pieces[x, y]; }
            set { _pieces[x, y] = value; }
        }

        public Piece this[Vector2Int position] {
            get { return _pieces[position.x, position.y]; }
            set { _pieces[position.x, position.y] = value; }
        }

        public void StartGame() {
            this[0,0] = new Rook();
        }

        public class PreviousStep {
            public Piece Piece;
            public Vector2Int PreviousPosition;
            public Vector2Int NewPosition;
        }
    }
}

