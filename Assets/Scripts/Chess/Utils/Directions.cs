using System.Collections.Generic;
using UnityEngine;

namespace Chess.Utils {
    public static class Directions {
        public static readonly List<Vector2Int> Diagonal = new()
            { new Vector2Int(1, 1), new Vector2Int(1, -1), new Vector2Int(-1, 1), new Vector2Int(-1, -1) };

        public static readonly List<Vector2Int> Straight = new()
            { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, -1) };
        
        public static readonly List<Vector2Int> Complete = new() { 
            new Vector2Int(1, 1), 
            new Vector2Int(1,-1), 
            new Vector2Int(-1,1), 
            new Vector2Int(-1, -1),
            new Vector2Int(0, 1), 
            new Vector2Int(1,0), 
            new Vector2Int(-1,0), 
            new Vector2Int(0, -1)
        };
        
        public static readonly List<Vector2Int> Knight = new() { 
            new Vector2Int(1, 2), 
            new Vector2Int(-1,2), 
            new Vector2Int(-2,1), 
            new Vector2Int(-2, -1),
            new Vector2Int(2, 1), 
            new Vector2Int(2,-1), 
            new Vector2Int(-1,-2), 
            new Vector2Int(1, -2)
        };
    }
}
