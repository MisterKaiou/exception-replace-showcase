using System;

namespace ExceptionReplacementShowcase.ResultRelated
{
    public abstract record Result<T>
    {
        protected T _value;

        public virtual bool Success { get; }
        public bool Failure => !Success;

        public Result(T value) => (_value, Success) = (value, true);

        protected Result() => (_value, Success) = (default, false);

        public virtual T GetValue() => _value is null
            ? throw new InvalidOperationException("Only successes can contain a value")
            : _value;

        public bool TryGetValue(out T value)
        {
            if (Failure)
            {
                value = default;
                return false;
            }

            value = _value;
            return true;
        }

        public TFinal Match<TFinal>(
            Func<T, TFinal> ifSuccess,
            Func<ErrorResult<T>, TFinal> ifError)
        {
            return Success
                ? ifSuccess(GetValue())
                : ifError(this as ErrorResult<T>);
        }

        public Result<TFinal> Map<TFinal>(Func<T, TFinal> operation)
        {
            return Success
                ? new SuccessResult<TFinal>(operation(GetValue()))
                : this is ErrorResult<T> a
                    ? new ErrorResult<TFinal>(a.ErrorMessage)
                    : throw new InvalidOperationException("Only mapping from ErrorResults are supported");
        }
    }

    public record SuccessResult<T> : Result<T>
    {
        public SuccessResult(T value) : base(value)
        {
        }
    }

    public record ErrorResult<T> : Result<T>
    {
        public string ErrorMessage { get; }

        public ErrorResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
