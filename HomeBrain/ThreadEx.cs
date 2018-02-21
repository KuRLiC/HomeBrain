using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace HomeBrain
{
    public static class ThreadEx
    {
        #region threads
        public static Runnable ExecThread(Action action)
        {
            return Run(false, action);
        }
        public static Runnable ExecThread<T0>(Action<T0> action, T0 t0)
        {
            return Run(false, action, t0);
        }
        public static Runnable ExecThread<T0, T1>(Action<T0, T1> action, T0 t0, T1 t1)
        {
            return Run(false, action, t0, t1);
        }
        public static Runnable ExecThread<T0, T1, T2>(Action<T0, T1, T2> action, T0 t0, T1 t1, T2 t2)
        {
            return Run(false, action, t0, t1, t2);
        }
        public static Runnable ExecThread<T0, T1, T2, T3>(Action<T0, T1, T2, T3> action, T0 t0, T1 t1, T2 t2, T3 t3)
        {
            return Run(false, action, t0, t1, t2, t3);
        }
        #endregion threads

        #region tasks
        public static Runnable ExecTask(Action action)
        {
            return Run(true, action);
        }
        public static Runnable ExecTask<T0>(Action<T0> action, T0 t0)
        {
            return Run(true, action, t0);
        }
        public static Runnable ExecTask<T0, T1>(Action<T0, T1> action, T0 t0, T1 t1)
        {
            return Run(true, action, t0, t1);
        }
        public static Runnable ExecTask<T0, T1, T2>(Action<T0, T1, T2> action, T0 t0, T1 t1, T2 t2)
        {
            return Run(true, action, t0, t1, t2);
        }
        public static Runnable ExecTask<T0, T1, T2, T3>(Action<T0, T1, T2, T3> action, T0 t0, T1 t1, T2 t2, T3 t3)
        {
            return Run(true, action, t0, t1, t2, t3);
        }
        #endregion tasks

        #region core
        public static Runnable Run(bool isTask, Delegate function, params object[] arguments)
        {
            var runnable = new Runnable();
            runnable.IsTask = isTask;
            runnable.Function = function;
            runnable.Arguments = arguments;
            return runnable.Start();
        }
        #endregion core

        #region utils
        public static void Noop()
        {
            SleepTicks(0);
        }

        public static void SleepMin(double s)
        {
            Sleep(TimeSpan.FromMinutes(s));
        }

        public static void SleepSec(double s)
        {
            Sleep(TimeSpan.FromSeconds(s));
        }

        public static void SleepMs(double ms)
        {
            Sleep(TimeSpan.FromMilliseconds(ms));
        }

        public static void SleepTicks(long ticks)
        {
            Sleep(TimeSpan.FromTicks(ticks));
        }

        public static void Sleep(TimeSpan time)
        {
            Thread.Sleep(time);
        }
        #endregion utils
    }
    public class Runnable : IDisposable
    {
        public Thread Runner = null;
        public ManualResetEvent FinishedEvent = new ManualResetEvent(false);
        public Delegate Function = null;
        public bool IsTask = true;
        public bool IsBackground = true;
        public ApartmentState ApartmentState = ApartmentState.STA;
        public object[] Arguments = null;
        public object Result = null;
        public Exception Error = null;
        public DateTime? Started = null, Stopped = null;

        public bool Finished { get { return Stopped != null; } }
        public TimeSpan? Elapsed { get { return Stopped != null && Started != null ? Stopped.Value - Started.Value : (TimeSpan?)null; } }

        public virtual Runnable Start()
        {
            if (Runner != null)
                throw new Exception("A runnable can only run once");
            if (IsTask)
            {
                ThreadPool.QueueUserWorkItem(ExecuteTask);
            }
            else
            {
                Runner = new Thread(Execute);
                Runner.IsBackground = IsBackground;
                Runner.SetApartmentState(ApartmentState);
                Runner.Start();
            }
            return this;
        }

        protected virtual void ExecuteTask(object obj)
        {
            Runner = Thread.CurrentThread;
            Execute();
        }

        protected virtual void Execute()
        {
            try
            {
                Started = DateTime.Now;
                Result = Function.DynamicInvoke(Arguments);
            }
            catch (Exception ex)
            {
                Error = ex;
            }
            finally
            {
                Stopped = DateTime.Now;
                FinishedEvent.Set();
            }
        }

        public virtual void Wait()
        {
            FinishedEvent.WaitOne();
        }

        public virtual void Dispose()
        {
            FinishedEvent.Dispose();
        }

    }
}
