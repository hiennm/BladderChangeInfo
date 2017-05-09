using Quartz;
using log4net;

using BladderChange.Service.Data.Model.Facades;

namespace BladderChange.Service
{
    class BladderChangeDataJob : IJob
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(BladderChangeDataService).Name);

        public void Execute(IJobExecutionContext context)
        {
            _logger.Info("Starting the data collection job...");
            var facade = new BladderChangeInfoFacade();
            var list = facade.GetActiveMachineList();
            facade.GetLastestBladderChangeInfo(list);
            facade.UpdateBladderChangeInfo(list);
            _logger.Info("Data collection job finished.");
        }
    }
}
