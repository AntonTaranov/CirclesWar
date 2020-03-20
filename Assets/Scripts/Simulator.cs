using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CirclesWar.Data;

namespace CirclesWar
{
    public class Simulator
    {
        List<CircleData> circles;
        Field2D fieldWithCells;

        float halfWidth;
        float halfHeight;

        bool started = false;

        readonly float destroyRadius;

        public Simulator(float width, float height, float unitDestroyRadius)
        {
            circles = new List<CircleData>();
            halfWidth = width * 0.5f;
            halfHeight = height * 0.5f;

            fieldWithCells = new Field2D(-halfWidth, -halfHeight, width, height, 5, 5);
            destroyRadius = unitDestroyRadius;
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
            foreach (var circle in circles)
            {
                circle.UpdatePosition(deltaTime);
            }
            var collisionPairs = fieldWithCells.FindOverlaps(circles);
            foreach(Pair<CircleData> collided in collisionPairs)
            {
                CollideCircles(collided);
            }
            var deadCircles = new List<CircleData>();
            foreach (var circle in circles)
            {
                if (circle.GetRadius() > destroyRadius)
                {
                    CheckCollisionWithWalls(circle, deltaTime);
                }
                else
                {
                    deadCircles.Add(circle);
                    circle.Kill();
                }
            }
        }

        private void BounceCircles(Pair<CircleData> pair, float distance)
        {
            var sumSpeed = pair.first.GetSpeed() - pair.second.GetSpeed();

            var updateTime = distance / sumSpeed.magnitude;

            pair.first.UpdatePosition(-updateTime);
            pair.second.UpdatePosition(-updateTime);

            var normalTwo = pair.first.GetPosition() - pair.second.GetPosition();

            normalTwo.Normalize();
            normalTwo -= sumSpeed.normalized;
            normalTwo.Normalize();
            var normalOne = normalTwo * -1;

            pair.first.HitWithNormal(normalOne);
            pair.first.UpdatePosition(updateTime);

            pair.second.HitWithNormal(normalTwo);
            pair.second.UpdatePosition(updateTime);
        }

        private void ShrinkPair(Pair<CircleData> pair, float distance)
        {
            if (distance > 0)
            {
                var contactNormal = pair.first.GetPosition() - pair.second.GetPosition();
                contactNormal.x = -contactNormal.x;
                var projectionOne = Mathf.Abs(Vector2.Dot(pair.first.GetSpeed(), contactNormal));
                var projectionTwo = Mathf.Abs(Vector2.Dot(pair.second.GetSpeed(), contactNormal));

                pair.first.Shrink(distance * projectionOne / (projectionOne + projectionTwo));
                pair.second.Shrink(distance * projectionTwo / (projectionOne + projectionTwo));
            }
        }

        private void CollideCircles(Pair<CircleData> pair)
        {
            var distance = pair.first.GetRadius() + pair.second.GetRadius() -
                (pair.first.GetPosition() - pair.second.GetPosition()).magnitude;

            if (pair.first.IsSameColor(pair.second))
            {
                BounceCircles(pair, distance);
            }
            else
            {
                ShrinkPair(pair, distance);
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
                circle.UpdatePosition(-updateTime);
            }
        }
           
    }
}
