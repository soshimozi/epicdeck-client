using EpiDeckClient.Services.Interfaces;
using System.Reflection;
using System;
using System.Linq;
using EpiDeckClient.Framework.Attributes;

namespace EpiDeckClient.Services
{
    public class BinarySerializationService : IBinarySerializationService
    {
        public T Deserialize<T>(byte[] buffer) where T : new()
        {
            var result = new T();

            var properties = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(prop => prop.GetCustomAttribute<PositionAttribute>() != null)
                .OrderBy(prop => prop.GetCustomAttribute<PositionAttribute>().Position)
                .ToList();

            var currentBytePosition = 0;

            foreach (var property in properties)
            {
                object value = null;
                var propType = property.PropertyType;

                if (propType == typeof(byte))
                {
                    value = buffer[currentBytePosition];
                    currentBytePosition += sizeof(byte);
                }
                else if (propType == typeof(sbyte))
                {
                    value = (sbyte)buffer[currentBytePosition];
                    currentBytePosition += sizeof(sbyte);
                }
                else if (propType == typeof(short))
                {
                    value = BitConverter.ToInt16(buffer, currentBytePosition);
                    currentBytePosition += sizeof(short);
                }
                else if (propType == typeof(ushort))
                {
                    value = BitConverter.ToUInt16(buffer, currentBytePosition);
                    currentBytePosition += sizeof(ushort);
                }
                else if (propType == typeof(int))
                {
                    value = BitConverter.ToInt32(buffer, currentBytePosition);
                    currentBytePosition += sizeof(int);
                }
                else if (propType == typeof(uint))
                {
                    value = BitConverter.ToUInt32(buffer, currentBytePosition);
                    currentBytePosition += sizeof(uint);
                }
                else if (propType == typeof(long))
                {
                    value = BitConverter.ToInt64(buffer, currentBytePosition);
                    currentBytePosition += sizeof(long);
                }
                else if (propType == typeof(ulong))
                {
                    value = BitConverter.ToUInt64(buffer, currentBytePosition);
                    currentBytePosition += sizeof(ulong);
                }
                else if (propType == typeof(float))
                {
                    value = BitConverter.ToSingle(buffer, currentBytePosition);
                    currentBytePosition += sizeof(float);
                }
                else if (propType == typeof(double))
                {
                    value = BitConverter.ToDouble(buffer, currentBytePosition);
                    currentBytePosition += sizeof(double);
                }
                else if (propType == typeof(bool))
                {
                    value = BitConverter.ToBoolean(buffer, currentBytePosition);
                    currentBytePosition += sizeof(bool);
                }
                else if (propType == typeof(char))
                {
                    value = BitConverter.ToChar(buffer, currentBytePosition);
                    currentBytePosition += sizeof(char);
                }
                else
                {
                    throw new InvalidOperationException($"Unsupported data type: {propType.Name}");
                }

                property.SetValue(result, value);
            }

            return result;
        }
    }
}