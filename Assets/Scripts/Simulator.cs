using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CirclesWar.Data;

namespace CirclesWar
{
    public class Simulator
    {
        List<CircleData> circles;

        float halfWidth;
        float halfHeight;

        bool started = false;

        public Simulator(float width, float height)
        {
            circles = new List<CircleData>();
            halfWidth = width * 0.5f;
            halfHeight = height * 0.5f;
        }

        public bool SpawnCircle(CircleData circle)
        {
            var circlePosition = circle.GetPosition();
            var circleRadius = circle.GetRadius();
            
            foreach (var circleOnField in circles)
            {
                var minDistance = circleRadius + circleOnField.GetRadius();
                var distance = (circlePosition - circleOnField.GetPosition()).magnitude;
                if (minDistance > distance)
                {
                    return false;
                }
            }
            circles.Add(circle);
            return true;
        }

        public void StartMoving(float minSpeed, float maxSpeed)
        {
            foreach(var circle in circles)
            {
                var speedValue = minSpeed + Random.value * (maxSpeed - minSpeed);
                var speed = new Vector2(Random.value - 0.5f, Random.value - 0.5f);
                speed.Normalize();
                circle.SetSpeed(speed * speedValue);               
            }

            started = true;
        }

        public void Update(float deltaTime)
        {
            if (!started) return;
            foreach(var circle in circles)
            {
                circle.UpdatePosition(deltaTime);
                CheckCollisionWithWalls(circle, deltaTime);
            }


        }

        private void CheckCollisionWithWalls(CircleData circle, float deltaTime)
        {
            var position = circle.GetPosition();
            var radius = circle.GetRadius();
            var invertX = false;
            var invertY = false;
            float deltaX = 0;
            float deltaY = 0;
            if (position.x + radius > halfWidth)
            {
                invertX = true;
                deltaX = halfWidth - position.x - radius;
            }
            else if (position.x - radius < -halfWidth)
            {
                invertX = true;
                deltaX = -halfWidth - position.x + radius;
            }

            if (position.y + radius > halfHeight)
            {
                invertY = true;
                deltaY = halfHeight - position.y - radius;
            }
            else if (position.y - radius < -halfHeight)
            {
                invertY = true;
                deltaY = -halfHeight - position.y + radius;
            }
            
            float updateTimeX = 0;
            float updateTimeY = 0;
            var speed = circle.GetSpeed();
            if(invertX)
            {
                updateTimeX = deltaX / speed.x;
            }
            if (invertY)
            {
                updateTimeY = deltaY / speed.y;
            }

            if (invertX || invertY)
            {
                var updateTime = Mathf.Min(updateTimeX, updateTimeY);
                circle.UpdatePosition(updateTime);
                circle.InvertSpeed(invertX, invertY);
                circle.UpdatePosition(deltaTime + updateTime);
            }

        }
           
    }
}
