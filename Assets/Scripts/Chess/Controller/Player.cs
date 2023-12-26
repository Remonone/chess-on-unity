using System.Collections.Generic;
using Chess.Pieces;
using UnityEngine;

namespace Chess.Controller {
    public class Player : MonoBehaviour {

        [SerializeField] private Board _board;
        [SerializeField] private GameObject _prefabTooltip;

        private PlayerSide _playerSide = PlayerSide.WHITE;

        private readonly List<GameObject> _selectionList = new();

        private Vector3 _position;
        private Piece _selectedPiece;

        private static Dictionary<string, Sprite> _tooltips;

        public void Update() {
            if (Input.GetMouseButtonUp(0)) {
                if (MovePiece()) {
                    ClearSelection();
                    return;
                }
                ClearSelection();
                SelectPiece();
            }
        }
        private bool MovePiece() {
            if (ReferenceEquals(_selectedPiece, null)) return false;
            var position = Input.mousePosition;
            var selectedPos = GetPositionBySelectingTooltip(position);
            if (selectedPos == (Vector2Int.one * -1)) return false;
            _board.MovePieceToNewPosition(_selectedPiece, selectedPos);
            _selectedPiece = null;
            return true;
        }
        
        private Vector2Int GetPositionBySelectingTooltip(Vector3 position) {
            _position = Camera.main.ScreenToWorldPoint(position);
            var boardPosition = _board.GetBoardPositionByWorldPosition(_position);
            var cellCenter = _board.GetWorldPositionByBoardPosition(boardPosition);
            var tooltip = _selectionList.Find(t => (t.transform.position - cellCenter).magnitude < .1f);
            if (ReferenceEquals(tooltip, null)) return Vector2Int.one * -1;
            return boardPosition;
        }

        private void SelectPiece() {
            var position = Input.mousePosition;
            var piece = GetPieceByMousePosition(position);

            var selectedPos = _board.GetBoardPositionByWorldPosition(_position);
            var tooltipSelection = Instantiate(_prefabTooltip, _board.GetWorldPositionByBoardPosition(selectedPos),
                Quaternion.identity);
            tooltipSelection.GetComponent<SpriteRenderer>().sprite = GetTooltip("selected");
            _selectionList.Add(tooltipSelection);

            if (ReferenceEquals(piece, null)) return;
            if (piece.ActiveSide != _playerSide) return;
            _selectedPiece = piece;
            var availablePositions = piece.GetMovePositions();

            foreach (var possiblePosition in availablePositions) {
                var pos = _board.GetWorldPositionByBoardPosition(possiblePosition.Position);
                CreateMoveTooltip(pos, possiblePosition);
            }
        }
        
        private void CreateMoveTooltip(Vector3 pos, PieceMove possiblePosition) {
            var dot = Instantiate(_prefabTooltip, pos, Quaternion.identity);
            dot.GetComponent<SpriteRenderer>().sprite = GetSpriteByPieceMove(possiblePosition);
            _selectionList.Add(dot);
        }
        private Sprite GetSpriteByPieceMove(PieceMove possiblePosition) {
            if (!possiblePosition.PieceUnderAttack) return GetTooltip("dot");
            return GetTooltip("attacked");
        }

        private static Sprite GetTooltip(string tooltipName) {
            if (_tooltips == null) {
                _tooltips = new Dictionary<string, Sprite>();
                foreach (var tooltip in Resources.LoadAll<Sprite>("")) {
                    _tooltips[tooltip.name] = tooltip;
                }
            }
            return _tooltips[tooltipName];
        }

        private Piece GetPieceByMousePosition(Vector3 mousePosition) {
            _position = Camera.main.ScreenToWorldPoint(mousePosition);
            var piecePosition = _board.GetBoardPositionByWorldPosition(_position);
            if (piecePosition.x is < 0 or > 7 || piecePosition.y is < 0 or > 7 ) return null;
            return _board[piecePosition];
        }

        private void ClearSelection() {
            foreach (var selectItem in _selectionList) {
                Destroy(selectItem);
            }
            _selectionList.Clear();
        }

        private void OnDrawGizmos() {
            Gizmos.DrawCube(_position, Vector3.one);
        }
    }
}
