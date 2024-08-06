using UnityEngine;

namespace IG.Level
{
    [CreateAssetMenu(fileName = "Level", menuName = "IG/Level")]
    public class LevelConfig : ScriptableObject
    {
        public enum GridType
        {
            Square = 4,
            Hexagonal = 6
        }

        public enum NodeType
        {
            WiFiNode,
            ComputerNode,
            CableNode,
            EmptyNode
        }

        [System.Serializable]
        public class NodeData
        {
            public GameObject nodePrefab; // Prefab for the node
            public NodeType nodeType;
            [Range(0, 5)] // Range is 0-5 to support 4 rotations for Square and 6 for Hexagonal
            public int initialRotation; // Number of rotations (0 for no rotation)
        }

        public GridType gridType = GridType.Square;
        public float nodeSize = 100f;
        public float spacing = 5f;

        /// <summary>
        /// Minimum number of moves for a perfect score
        /// </summary>
        public int minMoves;
        /// <summary>
        /// Maximum number of moves after which the score will be 1
        /// </summary>
        public int maxMoves;
        public int rows;
        public int columns;
        public NodeData[] grid;

        private void OnValidate()
        {
            ValidateGridSize();

            for (int i = 0; i < grid.Length; i++)
            {
                if (grid[i] == null)
                {
                    Debug.Log($"{name} {i}th element is null");
                }
            }
        }

        public void Initialize(Grid nodeParentGrid)
        {
            ValidateGridSize();

            nodeParentGrid.cellSize = new Vector2(nodeSize, nodeSize);
            nodeParentGrid.cellGap = new Vector2(spacing, spacing);
        }

        // TODO Grid size, row, column, Required nodes, other validations
        private void ValidateGridSize()
        {
            var gridSize = grid.Length;
            if (gridSize <= 0)
            {
                Debug.LogError($"{name} Grid size must be greater than 0.");
            }

            if (rows == 0 || columns == 0 || !gridSize.Equals(rows * columns))
            {
                Debug.LogError($"{name} Grid does not have valid rows or colums or Grid size is not valid");
            }
        }

        public void SetGridElement(int row, int column, NodeData element)
        {
            if (row >= 0 && row < rows && column >= 0 && column < columns)
            {
                int index = row * columns + column;
                grid[index] = element;
            }
            else
            {
                Debug.LogWarning($"{name} Grid position out of bounds");
            }
        }

        public NodeData GetGridElement(int row, int column)
        {
            if (grid.Length < 2 || row < 0 || row >= rows || column < 0 || column >= columns)
            {
                Debug.LogWarning($"{name} Grid position out of bounds");
                return null;
            }

            int index = row * columns + column;
            return grid[index];
        }
    }
}