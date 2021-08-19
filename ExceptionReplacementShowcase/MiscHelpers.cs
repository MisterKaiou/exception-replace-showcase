using System;

namespace ExceptionReplacementShowcase
{
    public static class MiscHelpers
    {
        public static TFinal Then<TInitial, TFinal>(this TInitial initial, Func<TInitial, TFinal> op)
        {
            return op(initial);
        }
    }
}
