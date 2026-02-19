using System.Text.Json.Serialization;

namespace Demo_WebAPI_EventAgenda.Presentation.WebApi.Dto.Response
{
    public class FaqResponseDto
    {
        
        public required long Id { get; set; }
        public required string Question { get; set; }
        public required string Response { get; set; }
        public required int NbLike { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public required bool IsHidden { get; set; }
    }
}

