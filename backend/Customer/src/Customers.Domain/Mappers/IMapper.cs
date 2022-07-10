namespace Customers.Domain.Mappers
{
    public interface IMapper<T, R>
    {
        public R? MapFrom(T? entity);
    }
}
