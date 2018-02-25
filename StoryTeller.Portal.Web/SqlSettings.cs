using StoryTeller.Portal.CQRS.Sql;
using System;
using System.IO;

namespace StoryTeller.Portal
{
    public class SqlSettings : ISqlSettings
    {
        public string ResultsDbConnStr { get; } = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Results;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
    }
}
