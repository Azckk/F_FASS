using AutoMapper;
using Common.EntityFramework.Extensions;
using Common.EntityFramework.Services.Interfaces;
using Common.Frame.Contexts;
using Common.Frame.Dtos.Frame;
using Common.NETCore.Extensions;
using Common.Service.Dtos;
using Common.Service.Entities;
using Common.Service.Service;
using FASS.Data.Consts.Warehouse;
using FASS.Data.Entities.Data;
using FASS.Data.Entities.Warehouse;
using FASS.Service.Consts.Data;
using FASS.Service.Entities.DataExtend;
using FASS.Service.Models.Report;
using FASS.Service.Services.Report.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FASS.Service.Services.Report
{
    public class DataService : AuditService<FrameContext, AuditEntity, AuditDto>, IDataService
    {
        public DataService(
            IUnitOfWork<FrameContext> unitOfWork,
            IRepository<FrameContext, AuditEntity> repository,
            IMapper mapper,
            IValidator<AuditDto> validator
        )
            : base(unitOfWork, repository, mapper, validator) { }

        public async Task<(DataTable, DataTable)> GetDataAsync(IDictionary<string, object?> where)
        {
            var sql1 = $"""
                WITH T1 AS (
                SELECT
                	T2.code car_code,
                	T1.state car_state,
                	T1.create_at,
                	ROW_NUMBER ( ) OVER ( ORDER BY T1.create_at ) ROW_NUM 
                FROM
                	record_diary T1
                	LEFT JOIN data_car T2 ON T1.code = T2.code
                	LEFT JOIN ( SELECT code, NAME FROM frame_dict_item WHERE dict_id = ( SELECT id FROM frame_dict WHERE code = 'CarState' ) ) T3 ON T1.code = T3.code 
                WHERE
                	T1.TYPE = 'CarState' 
                	AND T1.create_at >= @createAtStart 
                	AND T1.create_at <= @createAtEnd 
                {(where.ContainsKey("carId") ? " AND T1.code = @carId " : "")} 
                )
                """;

            var sql2 = """
                SELECT T2.car_code,
                    T2.car_state,
                    T2.create_at start_time,
                    T3.create_at end_time
                FROM T1 T2
                    LEFT JOIN T1 T3 ON T2.ROW_NUM = T3.ROW_NUM -1
                """;

            var sql3 = $"""
                {sql1}
                SELECT T1.*,
                    ROUND(DATE_PART('epoch',T1.end_time-T1.start_time)) diff_time 
                FROM ({sql2}) T1
                WHERE T1.start_time IS NOT NULL AND T1.end_time IS NOT NULL
                """;

            var sql4 = $"""
                {sql1}
                SELECT T1.car_code,
                    T1.car_state,
                    SUM(T1.diff_time) total_time
                FROM ({sql3}) T1
                GROUP BY T1.car_code,
                    T1.car_state
                """;

            var data1 = await Repository.GetDbConnection().ExecuteDataTableAsync(sql3, where);
            var data2 = await Repository.GetDbConnection().ExecuteDataTableAsync(sql4, where);

            return (data1, data2);
        }

        public async Task<DataTable> GetLoadAsync(IDictionary<string, object> where)
        {
            var sql = "";
            var data = await Repository.GetDbConnection().ExecuteDataTableAsync(sql);
            return data;
        }

        public async Task<DataTable> GetFaultAsync(IDictionary<string, object> where)
        {
            var sql = "";
            var data = await Repository.GetDbConnection().ExecuteDataTableAsync(sql);
            return data;
        }

        public async Task<dynamic> GetAlarmAsync(IDictionary<string, DateTime> where)
        {
            var sql1 = $"""
                SELECT COUNT(t5."name") as alarmcount, "name" as Name,t5.code 
                  FROM (
                     SELECT t3.name,t3.code FROM (
                        SELECT t1.id, t2."name", t2.create_at, t2.code
                          FROM "frame_dict" t1, frame_dict_item t2
                        WHERE 
                          t1.id = t2.dict_id AND t1.code = 'CarAlarm' 
                     ) t3 
                    RIGHT  JOIN record_mdcs_alarm t4 ON t3.code = t4.code and t4.create_at >= 
                      '{where["createAtStart"]}' AND t4.create_at <='{where["createAtEnd"]}'
                       ) t5
                 GROUP BY 
                    t5.name,t5.code ORDER BY alarmcount  
                """;
            var dataTable1 = await Repository.GetDbConnection().ExecuteDataTableAsync(sql1);
            var list = dataTable1.ToEntities<AlarmReportData>().OrderByDescending(e => e.AlarmCount).ToList();
            var pieChartList = list.Take(5).ToList();
            var columnarChartList = list.Take(5).ToList();
            var totalList = list.DeepClone();
            long total = list.Sum(e => e.AlarmCount);
            long other = total - pieChartList.Sum(e => e.AlarmCount);
            if (total == 0)
            {
                pieChartList.Clear();
                columnarChartList.Clear();
            }
            pieChartList.Add(new AlarmReportData
            {
                AlarmCount = other,
                Name = "其他",
                Code = "other"
            });
            totalList.Add(new AlarmReportData
            {
                AlarmCount = total,
                Name = "总故障",
                Code = "total"
            });
            var data = new
            {
                totalData = totalList,
                pieChartData = pieChartList,
                columnarChartData = columnarChartList
            };
            return data;
        }

        /// <summary>
        /// 获取储位信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public async Task<dynamic> GetStorageAsync()
        {
            var storages = await UnitOfWork.GetRepository<StorageEntity>().ToListAsync(e => e.IsEnable);
            List<dynamic> list = new List<dynamic>();
            list.Add(new
            {
                name = "空料库位",
                value = storages.Where(e => e.IsEnable
                        && e.State == StorageConst.State.EmptyContainer
                        && e.IsEnable).Count()
            });
            list.Add(new
            {
                name = "满料库位",
                value = storages.Where(e => e.IsEnable
                        && e.State == StorageConst.State.FullContainer
                        && e.IsEnable).Count()
            });
            list.Add(new
            {
                name = "空库位",
                value = storages.Where(e => e.IsEnable
                        && e.State == StorageConst.State.NoneContainer && e.IsEnable).Count()
            });
            return list;
        }


        public async Task<(DataTable, DataTable)> GetTaskAsync(IDictionary<string, object> where)
        {
            var sql0 = $"""
                select t5.create_at,
                case 
                when t5.state in ('Faulted','Canceled')
                then t5.state
                when t6.state in ('Faulted','Canceled')
                then t6.state
                when t6.state <> t5.state
                then 'Running'
                ELSE
                t5.state
                end as state
                from  (select * from flow_mdcs_task where call_mode  not like '%Exchange' or call_mode is null)  t5
                left join (select * from  flow_mdcs_task  where call_mode like '%Exchange') t6
                on  POSITION(t5.id IN t6.extend) > 0
                """;

            var sql = $"""
                WITH T1 AS (
                SELECT t1.state,t.generate_series as tasktime  
                FROM ({sql0}) t1 
                RIGHT JOIN (SELECT generate_series('{where["createAtStart"]}'::date, '{where[
                    "createAtEnd"
                ]}'::date, '1 day'::interval)::date) t 
                ON to_char(t1.create_at, 'YYYY-MM-DD') = to_char(t.generate_series,'YYYY-MM-DD')
                )
                """;
            var sql1 = $"""
                {sql}
                SELECT
                    DATE(tasktime) AS day,
                    SUM(CASE WHEN t1.state = 'Completed' THEN 1 ELSE 0 END) AS success,
                    SUM(CASE WHEN t1.state in('Faulted','Canceled')  THEN 1 ELSE 0 END) AS failure
                FROM
                    T1
                GROUP BY
                    day
                ORDER BY
                    day;               
                """;
            var sql2 = $"""
                {sql}
                SELECT (
                    SELECT count(T2.tasktime) as success
                    FROM T1 T2
                    WHERE T2.state='Completed'
                ) success, (
                    SELECT count(T3.tasktime) as failure
                    FROM T1 T3
                    WHERE T3.state in('Faulted','Canceled')
                ) failure, (
                    SELECT count(*) AS total
                    FROM T1
                    WHERE T1.state is not null
                ) total
                """;
            var dataTable1 = await Repository.GetDbConnection().ExecuteDataTableAsync(sql1);
            var dataTable2 = await Repository.GetDbConnection().ExecuteDataTableAsync(sql2);
            return (dataTable1, dataTable2);
        }

        public async Task<(DataTable, DataTable)> GetChargeConsumeAsync(Dictionary<string, object> where)
        {
            //充电能耗
            var sql = $"""
                SELECT SUM
                	( dn ) dn,
                	chargetime 
                FROM
                	(
                	SELECT
                		COALESCE(t1.consume_dn,0) AS dn,
                		T.generate_series AS chargetime 
                	FROM
                		report_charge_consume t1
                		RIGHT JOIN ( SELECT generate_series ( '{where["createAtStart"]}' :: DATE, '{where["createAtEnd"]}' :: DATE, '1 day' :: INTERVAL ) :: DATE ) T ON to_char( t1.create_at, 'YYYY-MM-DD' ) = to_char( T.generate_series, 'YYYY-MM-DD' ) 
                	) t2 
                GROUP BY
                	t2.chargetime 
                ORDER BY
                	t2.chargetime
                """;

            //放电能耗
            var sql1 = $"""
                SELECT SUM
                	( dn ) dn,
                	chargetime 
                FROM
                	(
                	SELECT
                        COALESCE(t1.consume_dn,0) AS dn,
                		T.generate_series AS chargetime 
                	FROM
                		report_discharge_consume t1
                		RIGHT JOIN ( SELECT generate_series ( '{where["createAtStart"]}' :: DATE, '{where["createAtEnd"]}' :: DATE, '1 day' :: INTERVAL ) :: DATE ) T ON to_char( t1.create_at, 'YYYY-MM-DD' ) = to_char( T.generate_series, 'YYYY-MM-DD' ) 
                	) t2 
                GROUP BY
                	t2.chargetime 
                ORDER BY
                	t2.chargetime
                """;
            var dataTable1 = await Repository.GetDbConnection().ExecuteDataTableAsync(sql);
            var dataTable2 = await Repository.GetDbConnection().ExecuteDataTableAsync(sql1);
            return (dataTable1, dataTable2);
        }

        public async Task insertAlarm()
        {
            Random random = new Random();
            string[] codes = { "1", "2", "3", "4", "5", "6", "7", "0" };

            for (int i = 0; i < 100; i++)
            {
                string id = i.ToString();
                string code = codes[random.Next(codes.Length)];
                string car_code = (codes[random.Next(codes.Length)] + 100).ToString();
                string car_name = (codes[random.Next(codes.Length)] + 100).ToString();
                string name = (codes[random.Next(codes.Length)] + 100).ToString();
                DateTime start_time = DateTime.Now.AddDays(-random.Next(365));
                DateTime end_time = start_time.AddHours(random.Next(24));
                bool is_finish = random.Next(2) == 1;
                double sort_number = 1;
                bool is_enable = random.Next(2) == 1;
                bool is_delete = random.Next(2) == 1;
                DateTime create_at = DateTime.Now;
                /*    string create_by = Guid.NewGuid().ToString().Substring(0, 50);*/
                DateTime update_at = DateTime.Now;
                /* string update_by = Guid.NewGuid().ToString().Substring(0, 50);
                 string remark = Guid.NewGuid().ToString();
                 string extend = Guid.NewGuid().ToString();*/

                string query = $"""INSERT INTO record_mdcs_alarm (id,car_code, car_name, code, name, start_time, end_time, is_finish, sort_number, is_enable, is_delete, create_at, create_by, update_at, update_by, remark, extend) VALUES ('{id}','{car_code}', '{car_name}', '{code}', '{name}', '{start_time}', '{end_time}', {is_finish}, {sort_number}, {is_enable}, {is_delete}, '{create_at}', '', '{update_at}', '', '', '')  """;
                var dataTable = await Repository.GetDbConnection().ExecuteDataTableAsync(query);
            }
        }

        public async Task<(dynamic, dynamic)> GetTaskCompletionRateAsync(IDictionary<string, object> where)
        {
            var sql0 = $"""
                select t5.create_at,
                case 
                when t5.state in ('Faulted','Canceled')
                then t5.state
                when t6.state in ('Faulted','Canceled')
                then t6.state
                when t6.state <> t5.state
                then 'Running'
                ELSE
                t5.state
                end as state
                from  (select * from flow_mdcs_task where call_mode  not like '%Exchange' or call_mode is null)  t5
                left join (select * from  flow_mdcs_task  where call_mode like '%Exchange') t6
                on  POSITION(t5.id IN t6.extend) > 0
                """;

            //时间区间类每天任务完成率
            var sql = $"""
                WITH T1 AS (
                SELECT t1.state,t.generate_series as tasktime  
                FROM ({sql0}) t1 
                RIGHT JOIN (SELECT generate_series('{where["createAtStart"]}'::date, '{where[
                    "createAtEnd"
                ]}'::date, '1 day'::interval)::date) t 
                ON to_char(t1.create_at, 'YYYY-MM-DD') = to_char(t.generate_series,'YYYY-MM-DD') 
                )
                """;
            var sql1 = $"""
                {sql}
                SELECT
                    DATE(tasktime) AS day,
                    SUM(CASE WHEN t1.state = 'Completed' THEN 1 ELSE 0 END) * 100 / COUNT(*) AS rate
                FROM
                    T1
                GROUP BY
                    day
                ORDER BY
                    day;               
                """;
            //当天任务完成情况
            var sql2 = $"""
                WITH T1 AS (
                   SELECT t.state, t.create_at::date as tasktime 
                   FROM ({sql0}) t 
                   WHERE t.create_at::date = CURRENT_DATE
                )
                SELECT
                    (
                        SELECT
                            COUNT(T2.tasktime) AS success
                        FROM
                            T1 T2
                        WHERE
                            T2.state = 'Completed'
                    ) success,
                    (
                        SELECT
                            COUNT(T3.tasktime) AS failure
                        FROM
                            T1 T3
                        WHERE
                            T3.state IN ('Faulted', 'Canceled')
                    ) failure,
                    (
                        SELECT
                            COUNT(*) AS total
                        FROM
                            T1
                        WHERE
                            T1.state IS NOT NULL
                    ) total                                
                """;

            var dataTable1 = await Repository.GetDbConnection().ExecuteDataTableAsync(sql1);
            var times = dataTable1.AsEnumerable().Select(row => row.Field<DateTime>("day").ToString("MM-dd")).ToArray();
            var rates = dataTable1.AsEnumerable().Select(row => row.Field<Int64>("rate").ToString()).ToArray();
            var LineChart = new
            {
                time = times,
                data = rates
            };
            var dataTable2 = await Repository.GetDbConnection().ExecuteDataTableAsync(sql2);
            return (LineChart, dataTable2.ToExpandoObjects());
        }

        public async Task<dynamic> GetCurrentDayAlarmAsync()
        {
            var sql1 = $"""
                SELECT 
                   COUNT(t5."name") AS alarmcount,
                	 "name" AS Name
                FROM 
                    (SELECT t3.name,
                		t3.code
                    FROM 
                        (SELECT t1.id,
                		 t2."name",
                		 t2.create_at,
                		 t2.code
                        FROM "frame_dict" t1, frame_dict_item t2
                        WHERE t1.id = t2.dict_id
                        		AND t1.code = 'CarAlarm' ) t3
                        INNER JOIN record_mdcs_alarm t4
                        	ON t3.code = t4.code
                        		AND t4.create_at::date = CURRENT_DATE ) t5
                    GROUP BY  t5.name,t5.code
                ORDER BY  alarmcount 
                """;
            var dataTable1 = await Repository.GetDbConnection().ExecuteDataTableAsync(sql1);
            var list = dataTable1.ToEntities<AlarmReportData>().OrderByDescending(e => e.AlarmCount).ToList();
            var columnarChartList = list.Take(10).ToList();
            long total = list.Sum(e => e.AlarmCount);
            if (total == 0)
            {
                columnarChartList.Clear();
            }

            var sql2 = $"""
                SELECT
                    CODE,
                    NAME,
                    VALUE,
                    PARAM
                FROM
                    FRAME_DICT_ITEM T1
                WHERE
                    EXISTS (
                        SELECT
                            1
                        FROM
                            FRAME_DICT
                        WHERE
                            CODE = 'CarAlarm'
                            AND ID = T1.DICT_ID
                    )
                """;
            var alarmTable = await Repository.GetDbConnection().ExecuteDataTableAsync(sql2);
            var alarmList = alarmTable.ToEntities<DictItemDto>().ToList();
            string[] levelArr = new string[columnarChartList.Count];
            for (int i = 0; i < columnarChartList.Count; i++)
            {
                var level = alarmList.Where(e => e.Name == columnarChartList[i].Name).FirstOrDefault()?.Param;
                levelArr[i] = string.IsNullOrEmpty(level) ? "0" : level;
            }
            var data = new
            {
                columnarChartData = new
                {
                    data = columnarChartList.Select(e => e.AlarmCount).ToArray(),
                    name = columnarChartList.Select(e => e.Name).ToArray(),
                    alarmLevel = levelArr
                },
                totalAlarm = total
            };
            return data;
        }

        public async Task<dynamic> GetCarStateAsync()
        {
            var cars = await UnitOfWork.GetRepository<CarEntity>().ToListAsync(e => e.IsEnable);
            List<dynamic> list = new List<dynamic>();
            list.Add(new
            {
                name = "空闲",
                value = cars.Where(e => e.IsOnline == true
                        && e.IsNormal == true
                        && CarConstExtend.State.Idle.Contains(e.CurrState)).Count()
            });
            list.Add(new
            {
                name = "任务中",
                value = cars.Where(e => e.IsOnline == true
                        && e.IsNormal == true
                        && CarConstExtend.State.Executing.Contains(e.CurrState)).Count()
            });
            list.Add(new
            {
                name = "充电中",
                value = cars.Where(e => e.IsOnline == true
                        && e.IsNormal == true
                        && e.CurrState == CarConstExtend.State.Charging).Count()
            });
            list.Add(new
            {
                name = "异常",
                value = cars.Where(e => e.IsOnline == true
                        && e.IsNormal == true
                        && CarConstExtend.State.Malfunction.Contains(e.CurrState)).Count()
            });
            list.Add(new
            {
                name = "离线",
                value = cars.Where(e => e.IsOnline == false || e.IsNormal == false).Count()
            });
            return list;
        }

        public async Task<dynamic> GetChargeStateAsync()
        {
            var charges = await UnitOfWork.GetRepository<ChargingStationEntity>().ToListAsync(e => e.IsEnable);
            List<dynamic> list = new List<dynamic>();
            list.Add(new
            {
                name = "充电中",
                value = charges.Where(e => e.IsEnable
                        && e.State == ChargingStationConst.State.Online
                        && e.IsOccupied).Count()
            });
            list.Add(new
            {
                name = "空闲中",
                value = charges.Where(e => e.IsEnable
                        && e.State == ChargingStationConst.State.Online
                        && !e.IsOccupied).Count()
            });
            list.Add(new
            {
                name = "离线",
                value = charges.Where(e => e.IsEnable
                        && e.State == ChargingStationConst.State.Offline).Count()
            });
            return list;
        }

    }
}
