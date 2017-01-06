using System;
using System.Diagnostics;
using System.Threading;

namespace Aliyun.Api.LOG.Common
{
    /// <summary>
    /// The implementation of <see cref="IAsyncResult"/>
    /// that represents the status of an async operation.
    /// </summary>
    internal abstract class AsyncResult : IAsyncResult, IDisposable
    {
        #region Fields

        private object _asyncState;
        private bool _completedSynchronously;
        private bool _isCompleted;
        private AsyncCallback _userCallback;
        private ManualResetEvent _asyncWaitEvent;
        private Exception _exception;

        #endregion

        #region IAsyncResult Members

        /// <summary>
        /// Gets a user-defined object that qualifies or contains information about an asynchronous operation.
        /// </summary>
        [DebuggerNonUserCode]
        public object AsyncState
        {
            get
            {
                return _asyncState;
            }
        }

        /// <summary>
        /// Gets a <see cref="WaitHandle"/> that is used to wait for an asynchronous operation to complete. 
        /// </summary>
        [DebuggerNonUserCode]
        public WaitHandle AsyncWaitHandle
        {
            get
            {
                if (_asyncWaitEvent != null)
                {
                    return _asyncWaitEvent;
                }

                ManualResetEvent manualResetEvent = new ManualResetEvent(false);
                if (Interlocked.CompareExchange<ManualResetEvent>(ref _asyncWaitEvent, manualResetEvent, null) != null)
                {
                    manualResetEvent.Close();
                }
                if (this.IsCompleted)
                {
                    _asyncWaitEvent.Set();
                }

                return _asyncWaitEvent;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the asynchronous operation completed synchronously.
        /// </summary>
        [DebuggerNonUserCode]
        public bool CompletedSynchronously
        {
            get
            {
                return _completedSynchronously;
            }
            protected set
            {
                _completedSynchronously = value;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the asynchronous operation has completed.
        /// </summary>
        [DebuggerNonUserCode]
        public bool IsCompleted
        {
            get
            {
                return _isCompleted;
            }
        }

        #endregion

        /// <summary>
        /// Initializes an instance of <see cref="AsyncResult"/>.
        /// </summary>
        /// <param name="callback">The callback method when the async operation completes.</param>
        /// <param name="state">A user-defined object that qualifies or contains information about an asynchronous operation.</param>
        protected AsyncResult(AsyncCallback callback, object state)
        {
            _userCallback = callback;
            _asyncState = state;
        }

        /// <summary>
        /// Completes the async operation with an exception.
        /// </summary>
        /// <param name="ex">Exception from the async operation.</param>
        public void Complete(Exception ex)
        {
            // Complete should not throw if disposed.
            Debug.Assert(ex != null);
            _exception = ex;
            this.NotifyCompletion();
        }

        /// <summary>
        /// When called in the dervied classes, wait for completion.
        /// It throws exception if the async operation ends with an exception.
        /// </summary>
        protected void WaitForCompletion()
        {
            if (!this.IsCompleted)
            {
                _asyncWaitEvent.WaitOne();
            }

            if (this._exception != null)
            {
                throw this._exception;
            }
        }

        /// <summary>
        /// When called in the derived classes, notify operation completion
        /// by setting <see cref="P:AsyncWaitHandle"/> and calling the user callback.
        /// </summary>
        [DebuggerNonUserCode]
        protected void NotifyCompletion()
        {
            _isCompleted = true;
            if (_asyncWaitEvent != null)
            {
                _asyncWaitEvent.Set();
            }

            if (_userCallback != null)
            {
                _userCallback(this);
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Disposes the object and release resource.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// When overrided in the derived classes, release resources.
        /// </summary>
        /// <param name="disposing">Whether the method is called <see cref="M:Dispose"/></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && _asyncWaitEvent != null)
            {
                _asyncWaitEvent.Close();
                _asyncWaitEvent = null;
            }
        }

        #endregion
    }

    /// <summary>
    /// Represents the status of an async operation.
    /// It also holds the result of the operation.
    /// </summary>
    /// <typeparam name="T">Type of the operation result.</typeparam>
    internal class AsyncResult<T> : AsyncResult
    {
        /// <summary>
        /// The result of the async operation.
        /// </summary>
        private T _result;

        /// <summary>
        /// Initializes an instance of <see cref="AsyncResult&lt;T&gt;"/>.
        /// </summary>
        /// <param name="callback">The callback method when the async operation completes.</param>
        /// <param name="state">A user-defined object that qualifies or contains information about an asynchronous operation.</param>
        public AsyncResult(AsyncCallback callback, object state)
            : base(callback, state)
        {
        }

        /// <summary>
        /// Gets result and release resources.
        /// </summary>
        /// <returns>The instance of result.</returns>
        public T GetResult()
        {
            base.WaitForCompletion();
            return _result;
        }

        /// <summary>
        /// Sets result and notify completion.
        /// </summary>
        /// <param name="result">The instance of result.</param>
        public void Complete(T result)
        {
            // Complete should not throw if disposed.
            this._result = result;
            base.NotifyCompletion();
        }
    }
}
