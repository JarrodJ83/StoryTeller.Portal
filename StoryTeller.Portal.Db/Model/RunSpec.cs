using System.ComponentModel.DataAnnotations.Schema;

namespace Storyteller.Portal.Db.Model
{
    [Table(nameof(RunSpec))]
    public class RunSpec
    {
        public int Id { get; set; }
        public virtual Spec Spec { get; set; }
        public virtual Run Run { get; set; }
        public bool? Passed { get; set; }
    }
}