using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathFinding
{

    public class Normalizer {

        static float GetLength(List<Vector3> source)
        {
            var distance = 0f;
            var count = source.Count;
            for (int i = 0; i < count - 1; i++) {
                distance += Vector3.Distance(source[i], source[i + 1]);
            }
            return distance;
        }

        public static List<Vector3> Normalize(List<Vector3> source, int n, out float length)
        {
            length = GetLength(source);

		    if (source.Count <= 1) return source;
            var unit = length / n;

            var v0 = source[0];
            var v1 = source[source.Count - 1];

            var samples = new List<Vector3>();
            samples.Add(v0);

            var total = 0f;
		    var prev_total = 0f;
            var prev = v0;
            var next_spot = unit;
            int index = 1;
            int count = 1;

            while (index != source.Count && count < n - 1)
            {
                var next = source[index];
                total += Vector3.Distance(prev, next);
                while (total >= next_spot) {
                    var sample = Vector3.Lerp(prev, next, (next_spot - prev_total) / (total - prev_total));
                    samples.Add(sample);
                    next_spot += unit;
                    count++;
                    if (count >= n - 1)
                    {
                        break;
                    }
                }
                prev = next;
                prev_total = total;
                index++;
            }

            samples.Add(v1);
            return samples;
        }

    }

}


