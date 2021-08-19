using System;

namespace ExceptionReplacementShowcase.ResultRelated
{
    public static class Result
    {
        public static Result<T> Ok<T>(T value) => new SuccessResult<T>(value);

        public static Result<T> Fail<T>(string errorMessage) => new ErrorResult<T>(errorMessage);
    }
}
