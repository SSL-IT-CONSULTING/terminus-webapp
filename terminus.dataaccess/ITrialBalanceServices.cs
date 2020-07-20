using System.Collections.Generic;
using System.Threading.Tasks;
using terminus.shared.models;

namespace terminus.dataaccess
{
    public interface ITrialBalanceServices
    {
        Task<bool> TrialBalanceInsert(rptTrialBalanceVM TrialBalance);

        Task<IEnumerable<rptTrialBalanceVM>> TrialBalanceList();
    }
}