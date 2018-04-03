using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathFinding
{

    public class PathBundle {

        public int Count { get { return count; } }
        public float Longest { get { return longest; } }

        public List<List<Vector3>> pathes;
        protected int count;
        protected float longest;

        public PathBundle(int length)
        {
            this.pathes = new List<List<Vector3>>();
            this.count = length;
        }

        public PathBundle(List<List<Vector3>> pathes)
        {
            this.pathes = pathes;
        }

        public void Add(List<Vector3> path)
        {
            float length;
            path = Normalizer.Normalize(path, count, out length);
            pathes.Add(path);
            longest = Mathf.Max(length, longest);
        }

        public Vector3[] GetData()
        {
            var data = new Vector3[count * pathes.Count];
            for (int i = 0, n = pathes.Count; i < n; i++)
            {
                var ioff = i * count;
                var path = pathes[i];
                for(int j = 0, m = path.Count; j < m; j++) {
                    data[ioff + j] = path[j];
                }
            }
            return data;
        }

    }

}


