using System.Threading;
using System.Threading.Tasks;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Commands;
using StoryTeller.ResultAggregation.Models;
using StoryTeller.ResultAggregation.Requests;

namespace StoryTeller.ResultAggregation.RequestHandlers
{
    public class AddSpecRequestHandler : IRequestHandler<Requests.AddSpecRequest, Spec>
    {
        private readonly ICommandHandler<AddSpec, int> _addSpeCommandHandler;

        public AddSpecRequestHandler(ICommandHandler<Commands.AddSpec, int> addSpeCommandHandler)
        {
            _addSpeCommandHandler = addSpeCommandHandler;
        }

        public async Task<Spec> HandleAsync(AddSpecRequest request, CancellationToken cancellationToken)
        {
            var spec = new Spec
            {
                AppId = request.AppId,
                Name = request.PostedSpec.Name,
                StoryTellerId = request.PostedSpec.StoryTellerId
            };

            var addSpecCmd = new Commands.AddSpec(spec);

            await _addSpeCommandHandler.ExecuteAsync(addSpecCmd, cancellationToken);

            return spec;
        }
    }
}
