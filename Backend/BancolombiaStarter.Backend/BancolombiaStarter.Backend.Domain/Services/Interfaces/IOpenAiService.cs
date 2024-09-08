using BancolombiaStarter.Backend.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancolombiaStarter.Backend.Domain.Services.Interfaces
{
    public interface  IOpenAiService
    {
        Task<OpenAiResponse> GetCampaignSuggestionsAsync(string campaignDescription);

    }
}
