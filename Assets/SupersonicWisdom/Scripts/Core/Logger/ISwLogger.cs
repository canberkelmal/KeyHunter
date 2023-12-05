using System;

namespace SupersonicWisdomSDK
{
    internal interface ISwLogger
    {
        #region --- Properties ---

        bool LogViaNetwork { get; set; }

        #endregion


        #region --- Public Methods ---

        public void Setup(Func<bool> isDebugEnabledFunc);
        bool IsEnabled ();
        void Log(string message);
        void Log(Func<string> msgFn);

        void LogError(Exception message);
        void LogError(string message);
        void LogError(Func<string> msgFn);

        void LogWarning(string message);
        void LogWarning(Func<string> msgFn);

        #endregion
    }
}