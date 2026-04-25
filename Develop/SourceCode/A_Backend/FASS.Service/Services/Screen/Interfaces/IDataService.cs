using Common.Frame.Contexts;
using Common.Service.Dtos;
using Common.Service.Entities;
using Common.Service.Services.Interfaces;
using System.Data;

namespace FASS.Service.Services.Screen.Interfaces
{
    public interface IDataService : IAuditService<FrameContext, AuditEntity, AuditDto>
    {
        IEnumerable<dynamic> getData1();
        dynamic getData2();
        IEnumerable<dynamic> getData3();
        IEnumerable<dynamic> getData4();
        IEnumerable<dynamic> getData5();
        IEnumerable<dynamic> getData6();
        Task<(DataTable, DataTable)> GetDataAsync(IDictionary<string, object> where);
        Task<DataTable> GetLoadAsync(IDictionary<string, object> where);
        Task<DataTable> GetFaultAsync(IDictionary<string, object> where);
        Task<dynamic> GetAlarmAsync(IDictionary<string, DateTime> where);
        Task<(DataTable, DataTable)> GetTaskAsync(IDictionary<string, object> where);
        Task<(DataTable, DataTable)> GetChargeConsumeAsync(Dictionary<string, object> where);

        Task insertAlarm();

        Task<(dynamic, dynamic)> GetTaskCompletionRateAsync(IDictionary<string, object> where);
        Task<dynamic> GetCurrentDayAlarmAsync();
        Task<(dynamic, dynamic)> GetCarStateAsync();
        Task<(dynamic, dynamic)> GetChargeStateAsync();
        Task<dynamic> GetStorageAsync();
    }
}
