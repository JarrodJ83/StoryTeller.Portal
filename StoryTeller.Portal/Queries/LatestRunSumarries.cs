using System;
using System.Collections.Generic;
using System.Text;
using StoryTeller.Portal.CQRS;
using StoryTeller.Portal.Models.Views;

namespace StoryTeller.Portal.Queries
{
    public class LatestRunSumarries : IQuery<List<RunSummary>>
    {
        public int[] AppIds { get; private set; }
        public LatestRunSumarries(int[] appIds = null)
        {
            AppIds = appIds;
        }
    }
}
