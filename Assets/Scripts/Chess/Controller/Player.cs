using System;
using Chess.Pieces;
using UnityEditor;
using UnityEngine;

namespace Chess.Controller {
    public class Player : MonoBehaviour {

        [SerializeField] private Board _board;

        private PlayerSide _playerSide = PlayerSide.WHITE;

        private const string PIECES_LAYER = "Pieces";

        private Vector3 _position;

        public void Update() {
            if (Input.GetMouseButtonUp(0)) {
                var position = Input.mousePosition;
                var piece = GetPieceByMousePosition(position);
                if (piece == null) return;
                if (piece.ActiveSide != _playerSide) return;
                var availablePositions = piece.GetPositions();
                foreach (var possiblePosition in availablePositions) {
                    print("Possible way: " + possiblePosition.Position + ". Piece:" + piece + ". Piece position: " + _board.GetPositionByWorldPosition(Camera.main.ScreenToWorldPoint(position)));
                }
            }
        }

        private Piece GetPieceByMousePosition(Vector3 mousePosition) {
            _position = Camera.main.ScreenToWorldPoint(mousePosition);
            var piecePosition = _board.GetPositionByWorldPosition(_position);
            if (piecePosition.x is < 0 or > 7 || piecePosition.y is < 0 or > 7 ) return null;
            return _board[piecePosition];
        }

        private void OnDrawGizmos() {
            Gizmos.DrawCube(_position, Vector3.one);
        }
    }
}
