using System.Reflection;
using System.Runtime.CompilerServices;

namespace FASS.Scheduler.Utility
{
    public class CommonHelper
    {
        //private static Dictionary<string, (DateTime dt, int state)> pressedDT = new Dictionary<string, (DateTime dt, int state)>();
        //// if active for millis, trigger once.
        //public static void TriggerOnce(bool active, int millis, Action trigger, int index = 0,
        //    [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        //{
        //    var id = $"{sourceFilePath}:{sourceLineNumber + index}";
        //    if (!active)
        //    {
        //        if (pressedDT.TryGetValue(id, out var pair1) && pair1.state != 1)
        //            pressedDT.Remove(id);
        //        return;
        //    }
        //    if (pressedDT.TryGetValue(id, out var pair))
        //    {
        //        if (pair.state != 0) return;
        //        if ((DateTime.Now - pair.dt).TotalMilliseconds > millis)
        //        {
        //            pressedDT[id] = (pair.dt, 1);
        //            Task.Run(() =>
        //            {
        //                try
        //                {
        //                    trigger();
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine($"Ex={ex.ToString()}", $"timed_trigger-{id}");
        //                    pressedDT.Remove(id);
        //                }

        //                pressedDT[id] = (pair.dt, 2);
        //            });
        //        }
        //    }
        //    else
        //        pressedDT[id] = (DateTime.Now, 0);
        //}
        //private static Dictionary<string, (DateTime dt, int state)> pressedDT = new Dictionary<string, (DateTime dt, int state)>();
        //public static void TriggerOnce(bool active, int millis, Action trigger,
        //  [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        //{
        //    var id = $"{sourceFilePath}:{sourceLineNumber}";
        //    if (!active)
        //    {
        //        if (pressedDT.TryGetValue(id, out var pair1) && pair1.state != 1)
        //            pressedDT.Remove(id);
        //        return;
        //    }
        //    if (pressedDT.TryGetValue(id, out var pair))
        //    {
        //        if (pair.state != 0) return;
        //        if ((DateTime.Now - pair.dt).TotalMilliseconds > millis)
        //        {
        //            pressedDT[id] = (pair.dt, 1);
        //            Task.Run(() =>
        //            {
        //                try
        //                {
        //                    trigger();
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine($"TriggerOnce err :{ex.Message}");
        //                    pressedDT.Remove(id);
        //                }

        //                pressedDT[id] = (pair.dt, 2);
        //            });
        //        }
        //    }
        //    else
        //        pressedDT[id] = (DateTime.Now, 0);
        //}

        private static Dictionary<string, (DateTime dt, int state)> pressedDT = new Dictionary<string, (DateTime dt, int state)>();
        // if active for millis, trigger once.
        public static void TriggerOnce(bool active, int millis, Action trigger, int index = 0,
            [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            var id = $"{sourceFilePath}:{sourceLineNumber + index}";
            if (!active)
            {
                if (pressedDT.TryGetValue(id, out var pair1) && pair1.state != 1)
                    pressedDT.Remove(id);
                return;
            }
            if (pressedDT.TryGetValue(id, out var pair))
            {
                if (pair.state != 0) return;
                if ((DateTime.Now - pair.dt).TotalMilliseconds > millis)
                {
                    pressedDT[id] = (pair.dt, 1);
                    Task.Run(() =>
                    {
                        try
                        {
                            trigger();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ex={ex.ToString()}", $"timed_trigger-{id}");
                            pressedDT.Remove(id);
                        }

                        pressedDT[id] = (pair.dt, 2);
                    });
                }
            }
            else
                pressedDT[id] = (DateTime.Now, 0);
        }

        private static Dictionary<MethodInfo, object> keepTrack = new Dictionary<MethodInfo, object>();
        /// <summary>
        /// including first call.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="getter"></param>
        /// <param name="action">action(T1 old)</param>
        //public static void TriggerIfChanged<T1>(Func<T1> getter, Action<T1> action)
        //{
        //    var id = getter.Method;
        //    var val = getter.Invoke();
        //    if (keepTrack.TryGetValue(id, out var t) && t is T1 old)
        //    {
        //        if (val.Equals(old)) return;
        //        action(old);
        //    }
        //    else action(default);
        //    keepTrack[id] = val;
        //}

        public static void FlipFlop<T1>(ref T1 target, int millis, params T1[] vs)
        {
            target = vs[(((long)DateTime.Now.TimeOfDay.TotalMilliseconds) / millis) % vs.Length];
        }

    }
}
