using Paramore.Brighter;

namespace Customers.Domain.Commands;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class PipelineAttribute : RequestHandlerAttribute
{
    private readonly Type _handler;

    public PipelineAttribute(int step, Type handler, HandlerTiming timing = HandlerTiming.Before)
        : base(step, timing)
    {
        _handler = handler;
    }

    public override Type GetHandlerType()
    {
        return _handler;
    }
}