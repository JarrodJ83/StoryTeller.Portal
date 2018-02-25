using System.ComponentModel.DataAnnotations.Schema;

namespace Storyteller.Portal.Db.Model
{
    [Table(nameof(RunResult))]
    public class RunResult
    {
        public int Id { get; set; }
        public string HtmlResults { get; set; }
        public virtual Run Run { get; set; }
        public bool? Passed { get; set; }
    }
}