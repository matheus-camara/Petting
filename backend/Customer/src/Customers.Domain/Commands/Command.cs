using Paramore.Brighter;
using System.Text.Json.Serialization;

namespace Customers.Domain.Commands;

public abstract record Command : ICommand
{
    private Guid? _id = null;

    [JsonIgnore]
    public Guid Id { get => _id ??= Guid.NewGuid(); set { } }
}