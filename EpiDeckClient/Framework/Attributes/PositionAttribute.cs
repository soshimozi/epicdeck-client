using System;

namespace EpiDeckClient.Framework.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PositionAttribute : Attribute
    {
        public int Position { get; }

        public PositionAttribute(int position)
        {
            Position = position;
        }
    }
}