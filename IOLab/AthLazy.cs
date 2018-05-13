using System;

namespace IOLab {
    public class AthLazy<T> {
        public T GetValue() {
            throw new NotImplementedException();
        }
        public AthLazy(Func<T> factory) { }
    }
}
