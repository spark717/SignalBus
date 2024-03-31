using System;

namespace Spark
{
    internal static class Log
    {
        public static void Excetion(Exception e)
        {
            if (SignalBusSettings.LogExceptions)
            {
#if UNITY_2020_1_OR_NEWER
                UnityEngine.Debug.LogException(e);
#else
            Console.WriteLine(e);
#endif
            }
            else
            {
                throw e;
            }
        }
    }
}