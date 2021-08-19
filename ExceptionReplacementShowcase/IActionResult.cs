namespace ExceptionReplacementShowcase
{
    public interface IActionResult
    {
        public object Body { get; }
    }

    public record ObjectResult(object Body) : IActionResult;

    public static class WannabeController
    {
        public static IActionResult Ok(object value) => new ObjectResult(value);
        public static IActionResult BadRequest(object value) => new ObjectResult(value);
    }
}
