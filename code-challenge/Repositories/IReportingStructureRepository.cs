using challenge.Models;
using System;

namespace challenge.Repositories
{
    public interface IReportingStructureRepository
    {
        ReportingStructure GetById(String id);
    }
}