using IG.Controller;
using IG.Level;
using IG.NodeSystem;
using UnityEngine;

namespace IG.Factory
{
    public static class NodeFactory
    {
        public static Node CreateNode(LevelConfig.NodeData nodeData, Transform parent, 
        int row, int column, LevelConfig.GridType gridType, IGrid gridManager)
        {
            if(nodeData.nodeType.Equals(LevelConfig.NodeType.EmptyNode))
            {
                Debug.Log($"Empty at position ({row}, {column})");
                
                // Instantiate the Empty node to fill the array space
                Object.Instantiate(nodeData.nodePrefab, parent);

                return null;
            }
            
            // Instantiate the Node
            var nodeObj = Object.Instantiate(nodeData.nodePrefab, parent);
            var node = nodeObj.GetComponent<Node>();

            var initialConnectibleSides = node.GetComponent<InitialConnectibleSides>();
            // We have to take the clone of array so it does not change the original
            var connectableSides = (bool[])initialConnectibleSides.connectibleSides.Clone();
            
            // Initialize the node with its data and apply the required rotation
            node.Initialize(row, column, connectableSides, gridManager, gridType);
            node.ApplyInitialRotation(nodeData.initialRotation);

            //TODO until we have proper level validation we check for level completion at start
            node.CheckConnections();

            return node;
        }
    }
}