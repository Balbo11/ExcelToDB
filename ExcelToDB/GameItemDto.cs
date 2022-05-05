using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ExcelToDB
{
    internal class GameItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public int ReleaseYear { get; set; }
        [JsonIgnore]
        public List<string> Publishers { get; set; }
    }
}
