using System.Threading;
using System.Threading.Tasks;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Models;
using StoryTeller.ResultAggregation.Queries;
using StoryTeller.ResultAggregation.Requests;

namespace StoryTeller.ResultAggregation.RequestHandlers
{
    public class GetLatestRunRequestHandler : IRequestHandler<GetLatestRunRequest, Run>
    {
        private IQueryHandler<LatestRunByApplication, Run> _latestRunByApplicationQueryHandler;

        public GetLatestRunRequestHandler(IQueryHandler<LatestRunByApplication, Run> latestRunByApplicationQueryHandler)
        {
            _latestRunByApplicationQueryHandler = latestRunByApplicationQueryHandler;
        }
        
        public async Task<Run> HandleAsync(GetLatestRunRequest request, CancellationToken cancellationToken)
        {
            return await _latestRunByApplicationQueryHandler.FetchAsync(new LatestRunByApplication(request.AppId), cancellationToken);
        }
    }
}
