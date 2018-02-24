namespace Storyteller.Portal.Db.Model
{
    public class RunResult
    {
        public int Id { get; set; }
        public string HtmlResults { get; set; }
        public virtual Run Run { get; set; }
    }
}