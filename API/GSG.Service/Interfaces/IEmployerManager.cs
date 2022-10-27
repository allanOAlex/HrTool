using GSG.Model;
using GSG.Model.DTO.Filters;

namespace GSG.Service.Interfaces
{
    public interface IEmployerManager
    {
        Employer CreateEmployer(Employer employer);
        bool DeleteEmployer(int employerId);
        IEnumerable<Employer> GetEmployers(EmployerFilterDTO filter);
        Employer GetEmployer(int employerId);
        Employer UpdateEmployer(Employer employer);
    }
}
