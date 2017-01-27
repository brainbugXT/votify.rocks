using Microsoft.WindowsAzure.Storage.Table;

namespace Votify.Rocks.Service.Models
{
    public class VoteSessionEntity : TableEntity
    {
       public string VoteSessionJSON { get; set; }
    }
}
