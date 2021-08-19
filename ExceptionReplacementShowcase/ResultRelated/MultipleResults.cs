using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace ExceptionReplacementShowcase.ResultRelated
{
    public abstract record MultipleResults<TExpected, TSuccesses, TErrors>
        : Result<IReadOnlyList<Result<TExpected>>>, IReadOnlyList<Result<TExpected>>
        where TSuccesses : SuccessResult<TExpected>
        where TErrors : ErrorResult<TExpected>
    {
        public int Count => _value.Count;
        public override bool Success { get; }

        public IReadOnlyList<TSuccesses> Successes { get; }
        public IReadOnlyList<TErrors> Errors { get; }

        public Result<TExpected> this[int index] => _value[index];

        protected MultipleResults(params Result<TExpected>[] results)
        {
            _value = results;
            Successes = results.Where(r => r.Success).Cast<TSuccesses>().ToImmutableList();
            Errors = results.Where(r => r.Failure).Cast<TErrors>().ToImmutableList();
            Success = Errors.Any() == false;
        }

        public IEnumerator<Result<TExpected>> GetEnumerator() => _value.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
