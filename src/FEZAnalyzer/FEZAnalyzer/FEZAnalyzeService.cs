using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace FEZAnalyzer
{
    public class FEZAnalyzeService : IDisposable
    {
        private CancellationTokenSource _cts  = null;
        private Task                    _task = null;

        private FEZAnalyzeService()
        { }

        public static async Task<FEZAnalyzeService> CreateAsync()
        {
            return await Task.Run(() =>
            {
                var fezAnalyzerService = new FEZAnalyzeService();

                return fezAnalyzerService;
            });
        }

        #region IDisposable 
        private bool _disposed = false;
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            try
            {
                Stop();

                if (_cts != null)
                {
                    _cts.Dispose();
                    _cts = null;
                }

                Trace.TraceInformation($"{nameof(FEZAnalyzeService)} is Disposed.");
            }
            finally
            {
                _disposed = true;
            }
        }
        #endregion

        /// <summary>
        /// 解析開始
        /// </summary>
        public void Start()
        {
            if (_disposed) throw new ObjectDisposedException($"{nameof(FEZAnalyzeService)}");

            if (_cts != null)
            {
                return;
            }

            try
            {
                _cts      = new CancellationTokenSource();
                var token = _cts.Token;
                _task     = Task.Run(() => { Run(token); }, token);

                Trace.TraceInformation($"{nameof(FEZAnalyzeService)} is started.");
            }
            catch
            {
                Stop();
                throw;
            }
        }

        /// <summary>
        /// 解析停止
        /// </summary>
        public void Stop()
        {
            if (_disposed) throw new ObjectDisposedException($"{nameof(FEZAnalyzeService)}");

            if (_cts == null)
            {
                return;
            }

            try
            {
                _cts.Cancel(false);
                _task.Wait();

                Trace.TraceInformation($"{nameof(FEZAnalyzeService)} is stopped.");
            }
            catch
            {
                throw;
            }
            finally
            {
                _cts.Dispose();
                _cts  = null;
                _task = null;
            }
        }

        private void Run(CancellationToken token)
        {
            var stopwatch    = Stopwatch.StartNew();
            var processTimes = new List<long>(100);

            while (!token.IsCancellationRequested)
            {
                try
                {
                    // 解析
                    var start  = stopwatch.ElapsedMilliseconds;
                    //var result = Analyze();
                    var end    = stopwatch.ElapsedMilliseconds;

                    // 解析時間更新
                    processTimes.Add(end - start);
                    if (processTimes.Count > 100)
                    {
                        //ProcessTimeUpdated?.Invoke(this, new ProcessTimeUpdatedEventArgs(processTimes.Average()));
                        processTimes.Clear();
                    }

                    // 処理が失敗している場合は大抵即終了している。
                    // ループによるCPU使用率を抑えるためにwait
                    var waitTime = 33 - (int)(end - start);
                    if (waitTime > 0)
                    {
                        Thread.Sleep(waitTime);
                    }
                }
                catch (OperationCanceledException)
                { }
                catch
                {
                    // 毎回出るエラーだった場合、エラーが出続けることになるため
                    // エラー発生時は終了する。
                    break;
                }
            }
        }
    }
}
