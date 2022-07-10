namespace Customers.Api.QueryParameters
{
    public class PageRequest
    {
        public int Skip { get; init; }
        public int Take { get; init; }

        public PageRequest(int? page, int? pageSize)
        {
            int sanitizedPage = ((page ?? 0) - 1) < 0 ? 0 : ((page ?? 0) - 1);

            Take = pageSize ?? 20;
            Skip = sanitizedPage * Take;
        }
    }
}
