using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ExcelToDB
{
    public class GameItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public int ReleaseYear { get; set; }
        public virtual ICollection<Publisher>? Publisher { get; set; }

    }
}
