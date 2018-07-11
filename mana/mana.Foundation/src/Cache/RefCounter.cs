namespace mana.Foundation
{
    public abstract class RefCounter
    {
        private int _refCount = 1;

        protected void Normalize()
        {
            _refCount = 1;
        }

        public void Retain()
        {
            _refCount++;
        }

        public void Release()
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
