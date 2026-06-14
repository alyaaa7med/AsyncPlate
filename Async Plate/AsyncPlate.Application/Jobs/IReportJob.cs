using AsyncPlate.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Jobs
{
    public interface IReportJob
    {

        Task ExecuteAsync();

    }
}
