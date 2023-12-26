using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chess.Pieces {
    public abstract class Piece : MonoBehaviour {

        [SerializeField] private List<SideSprites> _images = new(2);
        [SerializeField] private SpriteRenderer _image;

        protected internal PlayerSide ActiveSide;
        protected Board Board;
        protected Vector2Int Position;

        public Vector2Int GetPosition() => Position;

        private void Awake() {
            Board = FindObjectOfType<Board>();
        }

        public void Init(Vector2Int startPosition, PlayerSide side) {
            ActiveSide = side;
            Position = startPosition;
        }

        public void Start() {
            _image.sprite = GetSprite();
        }
        
        public abstract List<PieceMove> GetMovePositions();

        public Sprite GetSprite() => _images.First(image => image.Side == ActiveSide).Sprite;

        protected bool IsPointOutOfBound(Vector2Int newPosition) {
            return newPosition.x < 0 || newPosition.y < 0 || newPosition.x > 7 || newPosition.y > 7;
        }
        public void TranslatePosition(Vector2Int newPosition) {
            Position = newPosition;
            transform.Translate(Board.GetWorldPositionByBoardPosition(newPosition) - transform.position);
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
}