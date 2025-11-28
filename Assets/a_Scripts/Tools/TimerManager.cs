using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Timer
{
    public class Timer
    {
        public float Duration { get; private set; }
        public float StartTime { get; private set; }
        
       
        public Timer(float duration)
        {
            Duration = duration;
            StartTime = Time.time;
        }
    
        public void ReStart(float? newDuration = null)
        {
            Duration = newDuration ?? Duration;
            StartTime = Time.time;
        }
        
        public float Elapsed => Time.time - StartTime;
        public bool IsFinished => Elapsed >= Duration;
    }

    public class FrameTimer
    {
        public int MaxFrames { get; private set; }
        public int StartFrame { get; private set; }
        
        public int ElapsedFrames => Time.frameCount - StartFrame;
        public bool IsFinished => ElapsedFrames >= MaxFrames;
        
        // 帧计时转换为秒。
        public float ElapsedTime => ElapsedFrames * Time.deltaTime;
        public float DurationTime => MaxFrames * Time.deltaTime;

        public FrameTimer(int maxFrames)
        {
            MaxFrames = maxFrames;
            StartFrame = Time.frameCount;
        }
        
        public void ReStart(int? newMaxFrames = null)
        {
            MaxFrames = newMaxFrames ?? MaxFrames;
            StartFrame = Time.frameCount;
        }
    }
    
    public class TimerManager
    {
        private Dictionary<string, Timer> _timers = new();
        private Dictionary<string, FrameTimer> _frameTimers = new();
        
        //Time BASE
    
        public void Start(string key, float duration)
        {
            if (_timers.ContainsKey(key))
                _timers[key].ReStart(duration);
            else
                _timers[key] = new Timer(duration);
        }
    
        public bool IsFinished(string key)
        {
            return _timers.TryGetValue(key, out var timer) && timer.IsFinished;
        }

        public bool IsElapsedInRange(string key, float minTime, float maxTime)
        {
            if (_timers.TryGetValue(key, out var timer))
            {
                float elapsed = timer.Elapsed;
                return elapsed >= minTime && elapsed <= maxTime;
            }
            return false;
        }
    
        public float GetElapsed(string key)
        {
            return _timers.TryGetValue(key, out var timer) ? timer.Elapsed : 0f;
        }
    
        public void Restart(string key)
        {
            if (_timers.TryGetValue(key, out var timer))
                timer.ReStart(null);
        }

        // Frame Base
        public void Start(string key, int maxFrames)
        {
            if (_frameTimers.ContainsKey(key))
                _frameTimers[key].ReStart(maxFrames);
            else
                _frameTimers[key] = new FrameTimer(maxFrames);
        }

        public bool IsFrameFinished(string key)
        {
            return _frameTimers.TryGetValue(key, out var frameTimer) && frameTimer.IsFinished;
        }

        public bool IsFrameInRange(string key, int minFrames, int maxFrames)
        {
            if (_frameTimers.TryGetValue(key, out var frameTimer))
            {
                int elapsed = frameTimer.ElapsedFrames;
                return elapsed >= minFrames && elapsed <= maxFrames;
            }
            return false;
        }

        public int GetElapsedFrames(string key)
        {
            return _frameTimers.TryGetValue(key, out var frameTimer) ? frameTimer.ElapsedFrames : 0;
        }

        public float GetElapsedTimeFromFrames(string key)
        {
            return _frameTimers.TryGetValue(key, out var frameTimer) ? frameTimer.ElapsedTime : 0f;
        }

        public float GetDurationTimeFromFrames(string key)
        {
            return _frameTimers.TryGetValue(key, out var frameTimer) ? frameTimer.DurationTime : 0f;
        }

        public void RestartFrame(string key)
        {
            if (_frameTimers.TryGetValue(key, out var frameTimer))
                frameTimer.ReStart(null);
        }
    
        //Utilities
        
        public void Remove(string key)
        {
            _timers.Remove(key);
            _frameTimers.Remove(key);
        }
    
        public bool Exists(string key)
        {
            return _timers.ContainsKey(key) || _frameTimers.ContainsKey(key);
        }
        
        public void CleanupFinished()
        {
            var toRemove = new List<string>();
            foreach (var kvp in _timers)
                if (kvp.Value.IsFinished)
                    toRemove.Add(kvp.Key);
            foreach (var key in toRemove)
                _timers.Remove(key);
            
            toRemove.Clear();
            foreach (var kvp in _frameTimers)
                if (kvp.Value.IsFinished)
                    toRemove.Add(kvp.Key);
            foreach (var key in toRemove)
                _frameTimers.Remove(key);
        }
    }
}
