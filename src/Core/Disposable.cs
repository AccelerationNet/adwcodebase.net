using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Acceleration.Core {
    /// <summary>
    /// Easily create one-off `IDisposable` instances
    /// </summary>
    /// <remarks>Adapted from http://stackoverflow.com/questions/676746/custom-html-helpers-create-helper-with-using-statement-support</remarks>
    public sealed class DisposableHelper : IDisposable {
        Action onDone { get; set; }

        private DisposableHelper() { }

        // When the object is disposed (end of using block), write "end" function
        public void Dispose() {
            onDone();
        }

        public static IDisposable Create(Action onDone) {
            return new DisposableHelper() { onDone = onDone };
        }
    }

    /// <summary>
    /// Base class for `IDisposable` implementations
    /// </summary>
    /// <remarks>Follows guidelines in http://msdn.microsoft.com/en-us/library/aa720161%28v=vs.71%29.aspx </remarks>
    public abstract class Disposable : IDisposable {
        bool disposed;

        protected Disposable() { }

        /// <summary>
        /// dispose of unmanaged resources (Win32 crap like IntPtr) 
        /// </summary>
        protected virtual void DisposeUnManagedResources() { }
        /// <summary>
        /// dispose of managed resources (eg. DbConnection and stuff with Dispose or Close methods)
        /// </summary>
        protected abstract void DisposeManagedResources();


        /// <summary>
        /// Clean up resources
        /// </summary>
        /// <param name="disposing">true if called from `Dispose` or `using`, false if called from GC clean-up</param>
        void Dispose(bool disposing) {
            if (disposed) return;

            if (disposing) DisposeManagedResources();

            DisposeUnManagedResources();
            disposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Destructor to be called if the GC is cleaning us up
        /// </summary>
        ~Disposable() { Dispose(false); }
    }

}
