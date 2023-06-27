
using Elastic.Clients.Elasticsearch;

namespace DashboardService.DataAccess.Elastic;

public abstract class QueryAdapter<TQuery, TQueryResult, TIndex> where TIndex : class
{
    protected readonly TQuery query;

    protected QueryAdapter(TQuery query)
    {
        this.query = query;
    }

    public abstract SearchRequest<TIndex> BuildQuery();

    public abstract TQueryResult ExtractResult(SearchResponse<TIndex> searchResponse);
}