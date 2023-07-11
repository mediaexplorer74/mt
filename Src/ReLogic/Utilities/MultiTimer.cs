// MultiTimer

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ReLogic.Utilities
{
  public class MultiTimer
  {
    private int _ticksBetweenPrint;
    private int _ticksElapsedForPrint;
    private Stopwatch _timer = new Stopwatch();
    private Dictionary<string, MultiTimer.TimerData> _timerDataMap 
            = new Dictionary<string, MultiTimer.TimerData>();

        public MultiTimer(int ticksBetweenPrint = 100)
        {
            this._ticksBetweenPrint = ticksBetweenPrint;
        }

        public void Start()
    {
      this._timer.Reset();
      this._timer.Start();
    }

    public void Record(string key)
    {
      double totalMilliseconds = this._timer.Elapsed.TotalMilliseconds;
      MultiTimer.TimerData timerData;
      if (!this._timerDataMap.TryGetValue(key, out timerData))
        this._timerDataMap.Add(key, new MultiTimer.TimerData(totalMilliseconds));
      else
        this._timerDataMap[key] = timerData.AddTick(totalMilliseconds);
      this._timer.Restart();
    }

    public bool StopAndPrint()
    {
      ++this._ticksElapsedForPrint;
      if (this._ticksElapsedForPrint != this._ticksBetweenPrint)
        return false;
      this._ticksElapsedForPrint = 0;
      Debug.WriteLine("Average elapsed time: ");
      double num1 = 0.0;
      int val2 = 0;
      foreach (KeyValuePair<string, MultiTimer.TimerData> timerData in this._timerDataMap)
        val2 = Math.Max(timerData.Key.Length, val2);
      foreach (KeyValuePair<string, MultiTimer.TimerData> timerData1 in this._timerDataMap)
      {
        MultiTimer.TimerData timerData2 = timerData1.Value;
        string str = new string(' ', val2 - timerData1.Key.Length);
        object[] objArray = new object[11];
        objArray[0] = (object) timerData1.Key;
        objArray[1] = (object) str;
        objArray[2] = (object) " : (Average: ";
        double num2 = timerData2.Average;
        objArray[3] = (object) num2.ToString("F4");
        objArray[4] = (object) " Min: ";
        num2 = timerData2.Min;
        objArray[5] = (object) num2.ToString("F4");
        objArray[6] = (object) " Max: ";
        num2 = timerData2.Max;
        objArray[7] = (object) num2.ToString("F4");
        objArray[8] = (object) " from ";
        objArray[9] = (object) (int) timerData2.Ticks;
        objArray[10] = (object) " records)";

        Debug.WriteLine(string.Concat(objArray));
        num1 += timerData2.Total;
      }
      this._timerDataMap.Clear();
      Debug.WriteLine("Total : " + (object) (float) (num1 / (double) this._ticksBetweenPrint)
          + "ms");
      return true;
    }

    private struct TimerData
    {
      public readonly double Min;
      public readonly double Max;
      public readonly double Ticks;
      public readonly double Total;

      public double Average => this.Total / this.Ticks;

      private TimerData(double min, double max, double ticks, double total)
      {
        this.Min = min;
        this.Max = max;
        this.Ticks = ticks;
        this.Total = total;
      }

      public TimerData(double startTime)
      {
        this.Min = startTime;
        this.Max = startTime;
        this.Ticks = 1.0;
        this.Total = startTime;
      }

            public MultiTimer.TimerData AddTick(double time)
            {
                return new MultiTimer.TimerData(Math.Min(this.Min, time), 
                    Math.Max(this.Max, time), this.Ticks + 1.0, this.Total + time);
            }
        }
  }
}
