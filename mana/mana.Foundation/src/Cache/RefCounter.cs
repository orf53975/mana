namespace mana.Foundation
{
    public abstract class RefCounter
    {
        private int _refCount = 1;
        public int RefCount
        {
            get
            {
                return _refCount;
            }
        }

        protected void Normalize()
        {
            _refCount = 1;
        }

        public void Retain(object refOwner = null)
        {
            _refCount++;
        }

        public void Release(object refOwner = null)
        {
            _refCount--;
            if (_refCount < 0)
            {
                Logger.Error("RefCounter error! RefCount < 0");
            }
            else if (_refCount == 0)
            {
                OnReleased();
            }
        }

        protected abstract void OnReleased();

    }
}
