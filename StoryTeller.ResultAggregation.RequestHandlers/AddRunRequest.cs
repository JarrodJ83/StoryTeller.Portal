using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Commands;

namespace StoryTeller.ResultAggregation.RequestHandlers
{
    public class AddRunRequest : IRequestHandler<Requests.AddRunRequest, int>
    {
        private ICommandHandler<Commands.AddRunForApplication, int> addRunForApplication;

        public AddRunRequest(ICommandHandler<AddRunForApplication, int> addRunForApplication)
        {
            this.addRunForApplication = addRunForApplication;
        }

        public int Handle(Requests.AddRunRequest request)
        {
            int runId;
            addRunForApplication.Execute(new AddRunForApplication(request.ApplicationId, request.RunName, request.RunDateTime), out runId);
            return runId;
        }
    }
}
