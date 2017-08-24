using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Models;
using StoryTeller.ResultAggregation.Queries;
using StoryTeller.ResultAggregation.Requests;

namespace StoryTeller.ResultAggregation.RequestHandlers
{
    public class GetSpecsRequestHandler : IRequestHandler<Requests.GetAllSpecs, List<Spec>>
    {
        private IQueryHandler<Queries.SpecsByApplication, List<Spec>> _specsByApplicationQueryHandler;

        public GetSpecsRequestHandler(IQueryHandler<SpecsByApplication, List<Spec>> specsByApplicationQueryHandler)
        {
            _specsByApplicationQueryHandler = specsByApplicationQueryHandler;
        }

        public async Task<List<Spec>> HandleAsync(GetAllSpecs request, CancellationToken cancellationToken)
        {
            return await _specsByApplicationQueryHandler.FetchAsynx(new SpecsByApplication(request.ApplicationId), cancellationToken);
        }
    }
}
