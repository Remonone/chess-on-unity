using Chess.Pieces;

namespace Chess.Utils {
    public static class SideSwap {
        public static PlayerSide InvertSide(PlayerSide side) {
            return side == PlayerSide.BLACK ? PlayerSide.WHITE : PlayerSide.BLACK;
        }
    }
}
