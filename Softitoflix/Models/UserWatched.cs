using System.ComponentModel.DataAnnotations.Schema;

namespace Softitoflix.Models
{
    public class UserWatched
    {
        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public SoftitoflixUser? SoftitoflixUser { get; set; }

        public long EpisodeId { get; set; }
        [ForeignKey("EpisodeId")]
        public Episode? Episode { get; set; }
    }
}
