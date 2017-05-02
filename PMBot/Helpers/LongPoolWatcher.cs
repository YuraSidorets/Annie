using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using PMBot.BotServices;
using PMBot.Models;

namespace PMBot.Helpers
{
    public delegate void MessagesRecievedDelegate(VkService owner, IList<IList<object>> messages);

    public class LongPoolWatcher
    {
        private VkService _account;

        private int? Ts { get; set; }
        private int? Pts { get; set; }
        private string Server { get; set; }
        private string Key { get; set; }

        public bool Active { get; private set; }

        #region Управление слежением
        private Timer _watchTimer;

        public byte MaxSleepSteps = 3;
        public int SteepSleepTime = 333;
        private byte _currentSleepSteps = 1;
        #endregion

        public event MessagesRecievedDelegate NewMessages;

        public LongPoolWatcher(VkService api)
        {
            _account = api;
        }

        private LongPollServerResponse GetLongPoolServer(int? lastPts = null)
        {
            var response = _account.GetLongPollServer();

            Ts = response.Response.Ts;
            Pts = Pts == null ? response.Response.Pts : lastPts;
            Server = response.Response.Server;
            Key = response.Response.Key;

            return response;
        }
        private Task<LongPollServerResponse> GetLongPoolServerAsync(int? lastPts = null)
        {
            return Task.Run(() => { return GetLongPoolServer(lastPts); });
        }

        private LongPollServerHistory GetLongPoolHistory()
        {
            if (!Ts.HasValue)
                GetLongPoolServer();

            LongPollServerResponse rp = new LongPollServerResponse();
            rp.Response = new ServerHistoryResponse();
            if (Ts != null) rp.Response.Ts = Ts.Value;
            rp.Response.Pts = Pts;
            rp.Response.Server = Server;
            rp.Response.Key = Key;

            int c = 0;
            LongPollServerHistory history = null;
            string errorLog = "";

            while (c < 5 && history == null)
            {
                c++;
                try
                {
                    history = _account.GetLongPollHistory(rp);
                }
                catch (TooManyRequestsException)
                {
                    Thread.Sleep(150);
                    c--;
                }
                catch (Exception ex)
                {
                    errorLog += string.Format("{0} - {1}{2}", c, ex.Message, Environment.NewLine);
                }
            }

            if (history != null)
            {
                Ts = history.Ts;

            }
            else
                throw new NotImplementedException(errorLog);

            return history;
        }
        private Task<LongPollServerHistory> GetLongPoolHistoryAsync()
        {
            return Task.Run(() => { return GetLongPoolHistory(); });
        }

        private async void _watchAsync(object state)
        {
            var history = await GetLongPoolHistoryAsync();
            if (history?.Updates?.Count > 0)
            {
                _currentSleepSteps = 1;
                NewMessages?.Invoke(_account, history.Updates);
            }
            else if (_currentSleepSteps < MaxSleepSteps)
                _currentSleepSteps++;

            _watchTimer.Change(_currentSleepSteps * SteepSleepTime, Timeout.Infinite);
        }

        public async void StartAsync(int? lastTs = null, int? lastPts = null)
        {
            if (Active)
                throw new NotImplementedException("Messages for {0} already watching");

            Active = true;
            await GetLongPoolServerAsync(lastPts);

            _watchTimer = new Timer(_watchAsync, null, 0, Timeout.Infinite);
        }
        public void Stop()
        {
            _watchTimer?.Dispose();
            Active = false;
            _watchTimer = null;
        }
    }

    [Serializable]
    internal class TooManyRequestsException : Exception
    {
        public TooManyRequestsException()
        {
        }

        public TooManyRequestsException(string message) : base(message)
        {
        }

        public TooManyRequestsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TooManyRequestsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
