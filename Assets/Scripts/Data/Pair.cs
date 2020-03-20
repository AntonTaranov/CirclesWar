using System;
namespace CirclesWar.Data
{
    public class Pair<T>
    {
        public readonly T first;
        public readonly T second;

        public Pair(T first, T second)
        {
            this.first = first;
            this.second = second;
        }
    }
}
