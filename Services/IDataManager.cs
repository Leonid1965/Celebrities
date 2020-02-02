using Celebrities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Celebrities.Services
{
    public interface IDataManager
    {
        Task<Dictionary<string, Celebrity>> GetAllCelebrities();
        Task<Celebrity> GetSelebrity(string Id);
        Task<Dictionary<string, Celebrity>> UpdateSelebrity(Celebrity celebrity);
        Task<Dictionary<string, Celebrity>> DeleteSelebrity(string Id);
        Task<Dictionary<string, Celebrity>> AddSelebrity(Celebrity celebrity);
    }
}