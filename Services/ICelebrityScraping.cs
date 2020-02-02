using Celebrities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Celebrities.Services
{
    public interface ICelebrityScraping
    {
        Task<Dictionary<string, Celebrity>> CreateDataFile();
        void DeleteDataFile();
    }
}