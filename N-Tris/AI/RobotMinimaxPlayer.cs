using N_Tris.AI;
using N_TrisNetworkInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Tris
{
    public class RobotMinimaxPlayer : GamePlayer
    {

        // todo future improvement
        private Dictionary<int, Dictionary<HashBitArray, HashSet<int>>> rotsMap;

        private int depth = 2;
        private FitnessEvaluator fitness;
        private int previewSize = 5;

        private Node ansNode = new Node(-1, -1, 0, false);


        public RobotMinimaxPlayer()
        {
            rotsMap = new Dictionary<int, Dictionary<HashBitArray, HashSet<int>>>();
            fitness = new HoleFitness();
        }

        struct Node
        {
            public readonly int initialPosition;
            public readonly int inititalRotation;
            public readonly bool initialHold;
            public readonly int depth;
            public Node( int initialPosition, int initialRotation, int depth, bool initialHold )
            {
                this.initialPosition = initialPosition;
                this.inititalRotation = initialRotation; 
                this.depth = depth;
                this.initialHold = initialHold;
            }
        }



        public override HashSet<int> getMoves(GameBoardManipulator manager)
        {
            HashSet<int> ret = new HashSet<int>();

            if (ansNode.initialPosition == -1)
            {

                int ansScore = int.MinValue;

                Stack<KeyValuePair<GameBoardManipulator, Node>> S = new Stack<KeyValuePair<GameBoardManipulator, Node>>();

                S.Push(new KeyValuePair<GameBoardManipulator, Node>(manager.Clone(), new Node(-1, -1, 0, false)));

                while (S.Count > 0)
                {
                    KeyValuePair<GameBoardManipulator, Node> kv = S.Pop();
                    GameBoardManipulator data = kv.Key;
                    Node node = kv.Value;

                    int score = fitness.evaluate(data.Data);
                    if (node.inititalRotation != -1 && score > ansScore)
                    {
                        ansScore = score;
                        ansNode = node;
                    }

                    if (node.depth >= this.depth)
                    {
                        continue;
                    }

                    if (!data.Data.UsedHold)
                    {
                        GameBoardManipulator dataCopy = data.Clone();

                        dataCopy.holdPolyomino();
                        Node nextNode;
                        if (node.initialPosition == -1)
                        {
                            nextNode = new Node(0, 0, node.depth + 1, true);
                        }
                        else
                        {
                            nextNode = new Node(node.initialPosition, node.inititalRotation, node.depth + 1, node.initialHold);
                        }
                        S.Push(new KeyValuePair<GameBoardManipulator, Node>(dataCopy, nextNode));
                    }


                    for (int rot = 0; rot < 4; rot++)
                    {
                        for (int place = 0; place < data.Data.Width; place++)
                        {
                            GameBoardManipulator dataCopy = data.Clone();

                            bool validMove = true;
                            for (int i = 0; validMove && i < rot; i++)
                            {
                                validMove = validMove && dataCopy.tryRotateClockwise();
                            }

                            if (validMove)
                            {
                                validMove = validMove && dataCopy.tryFallingPosition(new Vector2(place, dataCopy.Data.FallingPolyominoLocation.Y));
                                if (validMove)
                                {
                                    // valid!
                                    int nextDepth = node.depth + 1;
                                    int nextInitPos = node.initialPosition;
                                    int nextInitRot = node.inititalRotation;
                                    if (node.initialPosition == -1)
                                    {
                                        nextInitPos = place;
                                        nextInitRot = (rot + data.Data.FallingPolyomino.rotation) % 4;
                                    }
                                    Node nextNode = new Node(nextInitPos, nextInitRot, nextDepth, node.initialHold);

                                    // hard drop
                                    while (dataCopy.FallingPolyominoDescend(true)) ;

                                    S.Push(new KeyValuePair<GameBoardManipulator, Node>(dataCopy, nextNode));

                                }
                            }
                        }
                    }
                }
            }
            if (ansNode.initialHold)
            {
                ansNode = new Node(-1, -1, 0, false);
                ret.Add((int)Constants.Moves.HOLD);
            }
            else if (ansNode.inititalRotation != manager.Data.FallingPolyomino.rotation)
            {
                ret.Add((int)Constants.Moves.ROTC);
            }
            else if (ansNode.initialPosition != manager.Data.FallingPolyominoLocation.X)
            {
                if (ansNode.initialPosition < manager.Data.FallingPolyominoLocation.X)
                {
                    ret.Add((int)Constants.Moves.LEFT);
                }
                else
                {
                    ret.Add((int)Constants.Moves.RIGHT);
                }
            }

            if (ret.Count == 0)
            {
                ansNode = new Node(-1, -1, 0, false);
                // hard drop
                ret.Add((int)Constants.Moves.HARD_DROP);
            }

            return ret;
        }

    }
}
