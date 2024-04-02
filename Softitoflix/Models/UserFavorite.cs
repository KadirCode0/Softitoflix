using System.ComponentModel.DataAnnotations.Schema;

namespace Softitoflix.Models
{
    public class UserFavorite
    {
        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public SoftitoflixUser? SoftitoflixUser { get; set; }

        public int MediaId { get; set; }
        [ForeignKey("MediaId")]
        public Media? Media { get; set; }
    }
}
