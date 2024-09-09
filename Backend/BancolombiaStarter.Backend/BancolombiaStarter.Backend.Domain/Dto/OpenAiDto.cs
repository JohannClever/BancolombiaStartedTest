using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancolombiaStarter.Backend.Domain.Dto
{

    public class OpenAiRequest
    {
        public string Prompt { get; set; }
    }
    public class OpenAiSuggestionProjectRequest
    {
        public List<long> SuggestionProjectsId { get; set; }
        public long ProjectId { get; set; }
    }

    public class OpenAiResponse
    {
        public string Id { get; set; }
        public string Object { get; set; }
        public string Created { get; set; }
        public OpenAiChoice[] Choices { get; set; }
    }

    public class OpenAiChoice
    {
        public string Text { get; set; }
        public int Index { get; set; }
        public object LogProbs { get; set; }
        public string FinishReason { get; set; }
    }

}
