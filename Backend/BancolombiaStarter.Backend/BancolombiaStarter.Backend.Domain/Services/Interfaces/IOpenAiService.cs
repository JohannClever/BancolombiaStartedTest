using BancolombiaStarter.Backend.Domain.Dto;
using BancolombiaStarter.Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancolombiaStarter.Backend.Domain.Services.Interfaces
{
    public interface  IOpenAiService
    {
        Task<OpenAiResponse> AskToIaAsync(string prompt);

    }
}
