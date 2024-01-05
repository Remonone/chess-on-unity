using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chess.Pieces {
    public abstract class Piece : MonoBehaviour {

        [SerializeField] private List<SideSprites> _images = new(2);
        [SerializeField] private SpriteRenderer _image;

        protected readonly PieceInfo _info = new();

        public PlayerSide Side => _info.Side;
        public Vector2Int Position => _info.Position;
        public Board Board => _info.Board;

        public PieceInfo Info => _info;
        

        private void Awake() {
            _info.Board = FindObjectOfType<Board>();
        }

        public void Init(Vector2Int startPosition, PlayerSide side) {
            _info.Side = side;
            _info.Position = startPosition;
            _info.Reference = this;
        }

        public void Start() {
            _image.sprite = GetSprite();
        }
        
        public abstract List<PieceMove> GetMovePositions(Table table, bool canSimulate);

        public Sprite GetSprite() => _images.First(image => image.Side == _info.Side).Sprite;

        protected bool IsPointOutOfBound(Vector2Int newPosition) {
            return newPosition.x < 0 || newPosition.y < 0 || newPosition.x > 7 || newPosition.y > 7;
        }
        public void TranslatePosition(Vector2Int newPosition) {
            _info.Position = newPosition;
            transform.Translate(_info.Board.GetWorldPositionByBoardPosition(newPosition) - transform.position);
        }

        protected List<PieceMove> GetDirectedPositions(Table table, List<Vector2Int> directions, bool canSimulate) {
            List<PieceMove> positions = new();
            foreach (var direction in directions) {
                for (int i= 1; i < 8; i++) {
                    Vector2Int newPosition = direction * i + _info.Position;
                    if (IsPointOutOfBound(newPosition)) break;
                    if (ReferenceEquals(table[newPosition.x, newPosition.y], null)) {
                        var move = new PieceMove { Position = newPosition };
                        if(canSimulate && _info.Board.IsKingAttackedOnSimulate(this, move)) continue;
                        positions.Add(move);
                        continue;
                    }
                    if (table[newPosition.x, newPosition.y].Side != _info.Side) {
                        var move = new PieceMove { Position = newPosition, PieceUnderAttack = table[newPosition.x, newPosition.y].Reference };
                        if(canSimulate && _info.Board.IsKingAttackedOnSimulate(this, move)) continue;
                        positions.Add(move);
                    }

                    break;
                }
            }

            return positions;
        }

    }

    [Serializable]
    internal sealed class SideSprites {
        public PlayerSide Side;
        public Sprite Sprite;
    }

    public sealed class PieceMove {
        public Vector2Int Position;
        public Piece PieceUnderAttack;
    }

    public sealed class PieceInfo {
        public Vector2Int Position;
        public Piece Reference;
        public PlayerSide Side;
        public Board Board;

        public PieceInfo Clone() {
            return new PieceInfo { Position = Position, Reference = Reference, Board = Board, Side = Side };
        }
    }
}