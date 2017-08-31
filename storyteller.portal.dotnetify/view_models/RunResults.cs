using DotNetify;
using DotNetify.Routing;

namespace storyteller.portal.dotnetify.view_models
{
    public class RunResults : BaseVM, IRoutable
    {
        public string HtmlResults { get; set; }
        public RoutingState RoutingState { get; set; }

        public RunResults()
        {
            this.OnRouted((sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.From))
                {
                    // Extract the book title from the route path.
                    var runId = e.From.ToLower().Replace("runresults/", "");

                    HtmlResults = "<div>these could be results!</div>"; // TODO: Get the run result
                    Changed(nameof(HtmlResults));
                }
            });
        }
    }
}
