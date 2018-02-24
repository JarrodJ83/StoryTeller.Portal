namespace Storyteller.Portal.Db.Model
{
    public class RunSpec
    {
        public virtual Spec Spec { get; set; }
        public virtual Run Run { get; set; }
        public bool Passed { get; set; }
    }
}