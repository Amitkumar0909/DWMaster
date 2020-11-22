using System;

namespace CodeSample.Logics.BLL
{
    public class BaseBLL : IDisposable
    { 
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(disposed);
            GC.SuppressFinalize(this);
        }
    }
}

