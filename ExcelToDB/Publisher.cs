using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ExcelToDB
{
    public class Publisher
    {
        public int id { get; set; }
        public string name { get; set; }
        /*[JsonIgnore]
        public virtual ICollection<GameItem> GameItem { get; set; }
        */
    }
}
