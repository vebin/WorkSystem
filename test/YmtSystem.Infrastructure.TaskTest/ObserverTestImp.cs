using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ParallelExtensionsExtras.Test
{
    public class ObserverTestImp<T> : IObservable<T>
    {
        private Action<T> _onNext;
        private Action<Exception> _onError;
        private Action _onCompleted;

        internal ObserverTestImp(Action<T> onNext, Action<Exception> onError, Action onCompleted)
        {
            if (onNext == null) throw new ArgumentNullException("onNext");
            if (onError == null) throw new ArgumentNullException("onError");
            if (onCompleted == null) throw new ArgumentNullException("onCompleted");
            _onNext = onNext;
            _onError = onError;
            _onCompleted = onCompleted;
        }

        public void OnCompleted() { _onCompleted(); }
        public void OnError(Exception error) { _onError(error); }
        public void OnNext(T value) { _onNext(value); }

        #region IObservable<T> 成员

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return new Unsubscriber<T>(new List<IObserver<T>>() { observer }, observer);
        }

        #endregion
    }
    class Unsubscriber<T> : IDisposable
    {
        private List<IObserver<T>> _observers;
        private IObserver<T> _observer;

        public Unsubscriber(List<IObserver<T>> observers,
            IObserver<T> observer)
        {
            this._observers = observers;
            this._observer = observer;
        }

        public void Dispose()
        {
            if (this._observer != null
                && this._observers.Contains(this._observer))
            {
                this._observers.Remove(_observer);
            }
        }
    }
}
