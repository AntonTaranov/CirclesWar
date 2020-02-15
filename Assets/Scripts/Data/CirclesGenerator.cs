using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CirclesWar.Data
{
    public class CirclesGenerator
    {
        List<CircleData> circles;
        private int counter = 0;

        public CirclesGenerator(int numCircles, float minRadius, float maxRadius)
        {
            circles = new List<CircleData>();           
            for (var i = 0; i < numCircles; i++)
            {
                var randomRadius = minRadius + Random.value * (maxRadius - minRadius);
                circles.Add(new CircleData(randomRadius, true));
                circles.Add(new CircleData(randomRadius, false));
            }
        }

        public CircleData GetNext()
        {
            counter++;           
            if (circles.Count >= counter)
            {
                return circles[counter - 1];
            }
            return null;
        }

    }
}
