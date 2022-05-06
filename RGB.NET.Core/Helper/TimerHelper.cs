using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace RGB.NET.Core;

/// <summary>
/// Offers some helper methods for timed operations.
/// </summary>
public static class TimerHelper
{
    #region DLL-Imports

    [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
    private static extern void TimeBeginPeriod(int t);

    [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
    private static extern void TimeEndPeriod(int t);

    #endregion

    #region Properties & Fields

    private static readonly object HIGH_RESOLUTION_TIMER_LOCK = new();

    private static bool _areHighResolutionTimersEnabled = false;
    private static int _highResolutionTimerUsers = 0;

    private static bool _useHighResolutionTimers = true;
    /// <summary>
    /// Gets or sets if High Resolution Timers should be used.
    /// </summary>
    public static bool UseHighResolutionTimers
    {
        get => _useHighResolutionTimers;
        set
        {
            lock (HIGH_RESOLUTION_TIMER_LOCK)
            {
                _useHighResolutionTimers = value;
                CheckHighResolutionTimerUsage();
            }
        }
    }

    // ReSharper disable once InconsistentNaming
    private static readonly HashSet<HighResolutionTimerDisposable> _timerLeases = new();

    #endregion

    #region Methods

    /// <summary>
    /// Executes the provided action and blocks if needed until the the <see param="targetExecuteTime"/> has passed.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <param name="targetExecuteTime">The time in ms this method should block. default: 0</param>
    /// <returns>The time in ms spent executing the <see param="action"/>.</returns>
    public static double Execute(Action action, double targetExecuteTime = 0)
    {
        long preUpdateTicks = Stopwatch.GetTimestamp();

        action();

        double updateTime = GetElapsedTime(preUpdateTicks);

        if (targetExecuteTime > 0)
        {
            int sleep = (int)(targetExecuteTime - updateTime);
            if (sleep > 0)
                Thread.Sleep(sleep);
        }

        return updateTime;
    }

    /// <summary>
    /// Calculates the ellapsed time in ms from the provided timestamp until now.
    /// </summary>
    /// <param name="initialTimestamp">The initial timestamp to calculate the time from.</param>
    /// <returns>The elapsed time in ms.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double GetElapsedTime(long initialTimestamp) => ((Stopwatch.GetTimestamp() - initialTimestamp) / (double)TimeSpan.TicksPerMillisecond);

    /// <summary>
    /// Requests to use to use High Resolution Timers if enabled.
    /// IMPORTANT: Always dispose the returned disposable if High Resolution Timers are no longer needed for the caller.
    /// </summary>
    /// <returns>A disposable to remove the request.</returns>
    public static IDisposable RequestHighResolutionTimer()
    {
        lock (HIGH_RESOLUTION_TIMER_LOCK)
        {
            _highResolutionTimerUsers++;
            CheckHighResolutionTimerUsage();
        }

        HighResolutionTimerDisposable timerLease = new();
        _timerLeases.Add(timerLease);
        return timerLease;
    }

    private static void CheckHighResolutionTimerUsage()
    {
        if (UseHighResolutionTimers && (_highResolutionTimerUsers > 0))
            EnableHighResolutionTimers();
        else
            DisableHighResolutionTimers();
    }

    private static void EnableHighResolutionTimers()
    {
        lock (HIGH_RESOLUTION_TIMER_LOCK)
        {
            if (_areHighResolutionTimersEnabled) return;

            // DarthAffe 06.05.2022: Linux should use 1ms timers by default
            if (OperatingSystem.IsWindows())
                TimeBeginPeriod(1);

            _areHighResolutionTimersEnabled = true;
        }
    }

    private static void DisableHighResolutionTimers()
    {
        lock (HIGH_RESOLUTION_TIMER_LOCK)
        {
            if (!_areHighResolutionTimersEnabled) return;

            if (OperatingSystem.IsWindows())
                TimeEndPeriod(1);

            _areHighResolutionTimersEnabled = false;
        }
    }

    /// <summary>
    /// Disposes all open High Resolution Timer Requests.
    /// This should be called once when exiting the application to make sure nothing remains open and the application correctly unregisters itself on OS level.
    /// Shouldn't be needed if everything is disposed, but better safe then sorry.
    /// </summary>
    public static void DisposeAllHighResolutionTimerRequests()
    {
        List<HighResolutionTimerDisposable> timerLeases = _timerLeases.ToList();
        foreach (HighResolutionTimerDisposable timer in timerLeases)
            timer.Dispose();
    }

    #endregion

    private class HighResolutionTimerDisposable : IDisposable
    {
        #region Properties & Fields

        private bool _isDisposed = false;

        #endregion

        #region Methods

        public void Dispose()
        {
            if (_isDisposed) return;

            _isDisposed = true;

            lock (HIGH_RESOLUTION_TIMER_LOCK)
            {
                _timerLeases.Remove(this);
                _highResolutionTimerUsers--;
                CheckHighResolutionTimerUsage();
            }
        }

        #endregion
    }
}