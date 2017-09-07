using System.Threading;
using System.Threading.Tasks;
using MediatR;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Commands;
using StoryTeller.ResultAggregation.Events;
using StoryTeller.ResultAggregation.Models;

namespace StoryTeller.ResultAggregation.RequestHandlers
{
    public class AddRunRequestHandler : Portal.CQRS.IRequestHandler<Requests.AddRunRequest, Run>
    {
        private readonly IMediator _mediator;
        private ICommandHandler<Commands.AddRunForApplication> _addRunForApplicationCommandHandler;
        private ICommandHandler<Commands.AddSpecToRun> _addSpecToRunCommandHandler;

        public AddRunRequestHandler(IMediator mediator, ICommandHandler<AddRunForApplication> addRunForApplicationCommandHandler, ICommandHandler<AddSpecToRun> addSpecToRunCommandHandler)
        {
            _mediator = mediator;
            _addRunForApplicationCommandHandler = addRunForApplicationCommandHandler;
            _addSpecToRunCommandHandler = addSpecToRunCommandHandler;
        }

        public async Task<Run> HandleAsync(Requests.AddRunRequest request, CancellationToken cancellationToken)
        {
            var run = new Run
            {
                Name = request.PostedRun.RunName,
                RunDateTime = request.PostedRun.RunDateTime
            };

            var addRunForApplicationCommand = new AddRunForApplication(request.AppId, run);

            await _addRunForApplicationCommandHandler.ExecuteAsync(addRunForApplicationCommand, cancellationToken);

            foreach (int specId in request.PostedRun.SpecIds)
            {
                await _addSpecToRunCommandHandler.ExecuteAsync(new AddSpecToRun(request.AppId, new RunSpec
                {
                    RunId = run.Id,
                    SpecId = specId
                }), cancellationToken);
            }
            
             await _mediator.Publish(new RunCreated(run.Id), cancellationToken);

            return run;
        }
    }
}
