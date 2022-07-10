namespace Customers.Domain.Mappers
{
    internal abstract class Mapper<T, R> : IMapper<T, R>
    {
        public R? MapFrom(T? entity)
        {
            return entity is { } e ? Map(e) : default(R?);
        }

        public IEnumerable<R?> MapFrom(IEnumerable<T?>? entity)
        {
            return entity?.Where(x => x is not null).Select(x => Map(x!)) ?? Enumerable.Empty<R>();
        }

        protected abstract R Map(T entity);
    }
}
