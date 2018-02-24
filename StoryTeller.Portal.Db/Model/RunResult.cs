namespace Storyteller.Portal.Db.Model
{
    public class RunResult
    {
        public string HtmlResults { get; set; }
        public virtual Run Run { get; set; }
    }
}