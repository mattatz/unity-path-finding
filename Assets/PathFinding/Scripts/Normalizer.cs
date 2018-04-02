using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathFinding
{

    public class Normalizer {

        public static List<Vector3> Normalize(List<Vector3> source, int n)
        {
            var count = source.Count;
            var distance = 0f;
            for (int i = 0; i < count - 1; i++) {
                distance += Vector3.Distance(source[i], source[i + 1]);
            }
            return Normalize(source, distance / n);
        }

        public static List<Vector3> Normalize(List<Vector3> source, float unit)
        {
            var points = new List<Vector3>();
            points.Add(source[0]);

            var cur = source[0];
            var index = 1;
            var count = source.Count;
            while(index < count)
            {
                var to = source[index] - cur;
                cur = cur + to.normalized * unit;
                points.Add(cur);
                if (to.magnitude < unit) { 
                    index++;
                }
            }
            return points;
        }

    }

}


