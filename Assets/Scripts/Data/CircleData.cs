using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CirclesWar.Data
{
    public class CircleData
    {
        Vector2 position;
        Vector2 speed;
        float radius;
        bool red;

        public CircleData(float radius, bool red)
        {
            this.radius = radius;
            this.red = red;
        }

        public bool IsSameColor(CircleData circle)
        {
            return circle.red == red;
        }

        public Color GetColor()
        {
            return red ? Color.red : Color.blue;
        }

        public float GetRadius()
        {
            return radius;
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public void SetPositionX(float x)
        {
            position.x = x;
        }

        public void SetPositionY(float y)
        {
            position.y = y;
        }

        public void SetSpeed(Vector2 value)
        {
            speed = value;
        }

        public Vector2 GetSpeed()
        {
            return speed;
        }

        public void UpdatePosition(float deltaTime)
        {
            position.x += deltaTime * speed.x;
            position.y += deltaTime * speed.y;
        }

        public void InvertSpeed(bool x, bool y)
        {
            speed.x = x ? -speed.x : speed.x;
            speed.y = y ? -speed.y : speed.y;
        }
    }
}