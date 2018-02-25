using System.ComponentModel.DataAnnotations.Schema;

namespace Storyteller.Portal.Db.Model
{
    [Table(nameof(App))]
    public class App 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ApiKey { get; set; }
    }
}