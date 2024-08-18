using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Random=System.Random;

namespace Common
{
    public static class CommonExtensions
    {
        private static readonly Random rand = new Random();


        public static T randomElement<T>(this Array array)
        {
            if (array.Length == 0)
                return default;

            int idx = rand.Next(0, array.Length);
            return (T)array.GetValue(idx);
        }

        public static T randomElement<T>(this IReadOnlyList<T> list)
            => randomElement(list, rand);

        public static T randomElement<T>(this IReadOnlyList<T> list, Random rnd)
        {
            if (list.Count == 0)
                return default;

            int idx = rnd.Next(0, list.Count);
            return list[idx];
        }

        public static T randomElement<T>(this T[] list)
            => randomElement(list, rand);

        public static T randomElement<T>(this T[] list, Random rnd)
        {
            if (list.Length == 0)
                return default;

            int idx = rnd.Next(0, list.Length);
            return list[idx];
        }

        public static T randomElement<T>(this IReadOnlyCollection<T> enumerable)
        {
            return enumerable.randomElement(new Random());
        }

        private static T randomElement<T>( this IReadOnlyCollection<T> enumerable, Random rand )
        {
            int index = rand.Next( 0, enumerable.Count );
            return enumerable.ElementAt( index );
        }

        public static TValue safeGet<TKey, TValue>( this IReadOnlyDictionary<TKey, TValue> dict, TKey key )
        {
            if ( dict != null && key != null && dict.TryGetValue( key, out TValue value ) )
                return value;
            return default;
        }

        public static List<T> forEach<T>( this IEnumerable<T> enumerable, Action<T> action )
        {
            return enumerable.Select(
                x =>
                {
                    action( x );
                    return x;
                }
            ).ToList();
        }
        
        public static T[] asArray<T>( this T item ) => new T[] { item };

        public static IEnumerable<Type> GetAllInheritorsOf<T>() where T : class
        {
            return Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T)));
        }

        public static T safeGet<T>( this IReadOnlyList<T> arr, int idx, bool lastIfOverflow = true )
        {
            if ( idx < 0 )
                return default;

            if ( arr == null || arr.Count == 0 )
                return default;

            if ( idx >= arr.Count )
                return lastIfOverflow ? arr[arr.Count - 1] : default;

            return arr[idx];
        }

        public static Vector3 setX( this Vector3 data, float x )
        {
            return new Vector3( x, data.y, data.z );
        }

        public static Vector3 setY( this Vector3 data, float y )
        {
            return new Vector3( data.x, y, data.z );
        }

        public static Vector3 setZ( this Vector3 data, float z )
        {
            return new Vector3( data.x, data.y, z );
        }


        public static Vector2 setX( this Vector2 data, float x )
        {
            return new Vector2( x, data.y );
        }

        public static Vector2 setY( this Vector2 data, float y )
        {
            return new Vector2( data.x, y );
        }

        public static Vector3 setZ( this Vector2 data, float z )
        {
            return new Vector3( data.x, data.y, z );
        }

        public static Vector3 plusX( this Vector3 data, float x )
        {
            return new Vector3( data.x + x, data.y, data.z );
        }

        public static Vector3 plusY( this Vector3 data, float y )
        {
            return new Vector3( data.x, data.y + y, data.z );
        }

        public static Vector3 plusZ( this Vector3 data, float z )
        {
            return new Vector3( data.x, data.y, data.z + z );
        }

        public static Vector3 plusX( this Vector2 data, float x )
        {
            return new Vector3( data.x + x, data.y, 0.0f );
        }

        public static Vector3 plusY( this Vector2 data, float y )
        {
            return new Vector3( data.x, data.y + y, 0.0f );
        }

        
        public static long withMin( this in long value, in long inclusiveMinimum )
        {
            if ( value < inclusiveMinimum )
                return inclusiveMinimum;

            return value;
        }

        public static long withMax( this in long value, in long inclusiveMaximum )
        {
            if ( value > inclusiveMaximum )
                return inclusiveMaximum;

            return value;
        }

        public static int withMin( this in int value, in int inclusiveMinimum )
        {
            if ( value < inclusiveMinimum )
                return inclusiveMinimum;

            return value;
        }

        public static int withMax( this in int value, in int inclusiveMaximum )
        {
            if ( value > inclusiveMaximum )
                return inclusiveMaximum;

            return value;
        }

        public static float withMin( this in float value, in float inclusiveMinimum )
        {
            if ( value < inclusiveMinimum )
                return inclusiveMinimum;

            return value;
        }

        public static float withMax( this in float value, in float inclusiveMaximum )
        {
            if ( value > inclusiveMaximum )
                return inclusiveMaximum;

            return value;
        }

        public static double withMin( this in double value, in double inclusiveMinimum )
        {
            if ( value < inclusiveMinimum )
                return inclusiveMinimum;

            return value;
        }

        public static double withMax( this in double value, in double inclusiveMaximum )
        {
            if ( value > inclusiveMaximum )
                return inclusiveMaximum;

            return value;
        }
    }
}
