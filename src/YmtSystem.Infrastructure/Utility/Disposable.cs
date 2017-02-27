
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Ymatou.Infrastructure
{
    public abstract class Disposable : IDisposable
    {
        private bool disposed;

        [DebuggerStepThrough]
        ~Disposable()
        {
            Dispose(false);
        }

        [DebuggerStepThrough]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [DebuggerStepThrough]
        protected virtual void InternalDispose()
        {
        }

        [DebuggerStepThrough]
        private void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                InternalDispose();
            }

            disposed = true;
        }
    }

    public abstract class SafeDisposable : SafeHandle
    {
        protected SafeDisposable()
            : base(IntPtr.Zero, true)
        { }

        protected virtual void InternalDispose()
        {

        }

        protected override void Dispose(bool disposing)
        {
            InternalDispose();
            base.Dispose(disposing);  
        }

        public override bool IsInvalid
        {
            get { return true ; }
        }

        protected override bool ReleaseHandle()
        {
            return true ;
        }
    }
}
