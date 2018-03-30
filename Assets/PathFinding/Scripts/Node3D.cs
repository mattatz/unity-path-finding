using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathFinding
{

    public class Node3D : Node {

        public Vector3 Position { get { return position; } }

        protected Vector3 position;

        public Node3D(Vector3 p) : base()
        {
            position = p;
        }

    }

}


