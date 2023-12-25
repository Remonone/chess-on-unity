using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chess.Pieces {
    public abstract class Piece : MonoBehaviour {

        [SerializeField] private List<SideSprites> _images = new(2);

        protected internal PlayerSide ActiveSide;
        protected Board Board;

        private void Awake() {
            Board = FindObjectOfType<Board>();
        }
        public abstract List<PieceMove> GetPositions(Vector2Int currentPosition);

        public Sprite GetSprite() => _images.First(image => image.Side == ActiveSide).Sprite;

        protected bool IsPointOutOfBound(Vector2 newPosition) {
            return newPosition.x < 0 || newPosition.y < 0 || newPosition.x > 7 || newPosition.y > 7;
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