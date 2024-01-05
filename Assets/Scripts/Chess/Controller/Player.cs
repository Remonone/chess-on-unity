using System.Collections.Generic;
using System.Linq;
using Chess.Pieces;
using Chess.Utils;
using UnityEngine;

namespace Chess.Controller {
    
    public class Player : MonoBehaviour {

        [SerializeField] private Board _board;
        [SerializeField] private GameObject _prefabTooltip;

        private PlayerSide _playerSide = PlayerSide.WHITE;

        private readonly Dictionary<GameObject, Piece> _selectionList = new();

        private Vector3 _position;
        private Piece _selectedPiece;

        private static Dictionary<string, Sprite> _tooltips;
        private Camera _camera;

        private void Start() {
            _camera = Camera.main;
        }
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
            var selectedPos = GetPositionBySelectingTooltip(position, out var tooltip);
            if (selectedPos == (Vector2Int.one * -1)) return false;
            _board.MovePieceToNewPosition(_selectedPiece, new PieceMove {Position = selectedPos, PieceUnderAttack = _selectionList[tooltip]});
            _selectedPiece = null;
            _playerSide = SideSwap.InvertSide(_playerSide);
            return true;
        }
        
        private Vector2Int GetPositionBySelectingTooltip(Vector3 position, out GameObject tooltip) {
            _position = _camera.ScreenToWorldPoint(position);
            var boardPosition = _board.GetBoardPositionByWorldPosition(_position);
            var cellCenter = _board.GetWorldPositionByBoardPosition(boardPosition);
            tooltip = _selectionList.Keys.ToList().Find(t => (t.transform.position - cellCenter).magnitude < .1f);
            if (ReferenceEquals(tooltip, null) || tooltip.GetComponent<SpriteRenderer>().sprite.name == "selected") return Vector2Int.one * -1;
            return boardPosition;
        }

        private void SelectPiece() {
            var position = Input.mousePosition;
            var piece = GetPieceByMousePosition(position);

            var selectedPos = _board.GetBoardPositionByWorldPosition(_position);
            var tooltipSelection = Instantiate(_prefabTooltip, _board.GetWorldPositionByBoardPosition(selectedPos),
                Quaternion.identity);
            tooltipSelection.GetComponent<SpriteRenderer>().sprite = GetTooltip("selected");
            _selectionList.Add(tooltipSelection, null);

            if (ReferenceEquals(piece, null)) return;
            if (piece.Side != _playerSide) return;
            _selectedPiece = piece;
            var availablePositions = _board.Table.Occupation[piece.Info]; // SAD :c

            foreach (var possiblePosition in availablePositions) {
                CreateMoveTooltip(possiblePosition);
            }
        }
        
        private void CreateMoveTooltip(PieceMove possiblePosition) {
            var pos = _board.GetWorldPositionByBoardPosition(possiblePosition.Position);
            var dot = Instantiate(_prefabTooltip, pos, Quaternion.identity);
            dot.GetComponent<SpriteRenderer>().sprite = GetSpriteByPieceMove(possiblePosition);
            _selectionList.Add(dot, possiblePosition.PieceUnderAttack);
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
            _position = _camera.ScreenToWorldPoint(mousePosition);
            var piecePosition = _board.GetBoardPositionByWorldPosition(_position);
            if (piecePosition.x is < 0 or > 7 || piecePosition.y is < 0 or > 7 ) return null;
            return _board.Table[piecePosition]?.Reference; // SAD :c
        }

        private void ClearSelection() {
            foreach (var selectItem in _selectionList) {
                Destroy(selectItem.Key);
            }
            _selectionList.Clear();
        }

        private void OnDrawGizmos() {
            Gizmos.DrawCube(_position, Vector3.one);
        }
    }
}
