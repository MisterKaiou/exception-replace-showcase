using System;
using System.Collections.Generic;
using System.Linq;
using ExceptionReplacementShowcase.Core;

namespace ExceptionReplacementShowcase.ResultRelated
{
    public record PersonBatchMappingResult : MultipleResults<Person, SuccessResult<Person>, ErrorResult<Person>>
    {
        public PersonBatchMappingResult(params Result<Person>[] results) : base(results)
        {
        }
    }
}
