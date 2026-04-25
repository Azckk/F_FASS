using AutoMapper;
using Common.Frame.Contexts;
using Common.Frame.Dtos.Account;
using Common.Frame.Dtos.Frame;
using Common.Frame.Entities.Account;
using Common.Frame.Entities.Frame;
using Common.Frame.Options;
using Common.NETCore.Helpers;
using FASS.Data.Dtos.Data;
using FASS.Data.Entities.Data;
using FASS.Data.Extensions;
using FASS.Service.Dtos.FlowExtend;
using FASS.Service.Entities.FlowExtend;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FASS.Service.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddService(this IServiceCollection services, IConfiguration configuration, string activationCode, Func<FrameOption>? setupAction = null)
        {
            services.AddData(configuration, activationCode, setupAction);
            services.AddScoped<Services.Warehouse.Interfaces.IAreaService, Services.Warehouse.AreaService>();
            services.AddScoped<Services.Warehouse.Interfaces.IStorageService, Services.Warehouse.StorageService>();
            services.AddScoped<Services.Warehouse.Interfaces.IContainerService, Services.Warehouse.ContainerService>();
            services.AddScoped<Services.Warehouse.Interfaces.IMaterialService, Services.Warehouse.MaterialService>();
            services.AddScoped<Services.Warehouse.Interfaces.IStorageContainerHistoryService, Services.Warehouse.StorageContainerHistoryService>();
            services.AddScoped<Services.Warehouse.Interfaces.IContainerMaterialHistoryService, Services.Warehouse.ContainerMaterialHistoryService>();
            services.AddScoped<Services.Warehouse.Interfaces.IMaterialStorageHistoryService, Services.Warehouse.MaterialStorageHistoryService>();
            services.AddScoped<Services.Warehouse.Interfaces.IWorkService, Services.Warehouse.WorkService>();
            services.AddScoped<Services.Warehouse.Interfaces.ITagService, Services.Warehouse.TagService>();
            services.AddScoped<Services.Warehouse.Interfaces.IStorageTagService, Services.Warehouse.StorageTagService>();
            services.AddScoped<Services.Warehouse.Interfaces.IPreMaterialService, Services.Warehouse.PreMaterialService>();
            services.AddScoped<Services.Warehouse.Interfaces.IPreWorkService, Services.Warehouse.PreWorkService>();
            services.AddScoped<Services.Setting.Interfaces.IConfigServiceService, Services.Setting.ConfigServiceService>();
            services.AddScoped<Services.Setting.Interfaces.IConfigChargeService, Services.Setting.ConfigChargeService>();
            services.AddScoped<Services.Report.Interfaces.IDataService, Services.Report.DataService>();
            services.AddScoped<Services.Screen.Interfaces.IDataService, Services.Screen.DataService>();
            services.AddScoped<Services.Custom.Interfaces.IDemoService, Services.Custom.DemoService>();
            services.AddScoped<Services.DataExtend.Interfaces.IEnvelopeService, Services.DataExtend.EnvelopeService>();
            services.AddScoped<Services.DataExtend.Interfaces.ITrafficRulesService, Services.DataExtend.TrafficRulesService>();
            services.AddScoped<Services.DataExtend.Interfaces.IPlanRulesService, Services.DataExtend.PlanRulesService>();
            services.AddScoped<Services.DataExtend.Interfaces.IAttributeService, Services.DataExtend.AttributeService>();
            services.AddScoped<Services.DataExtend.Interfaces.IChargingStationService, Services.DataExtend.ChargingStationService>();
            services.AddScoped<Services.DataExtend.Interfaces.ICarZoneService, Services.DataExtend.CarZoneService>();
            services.AddScoped<Services.BaseExtend.Interfaces.IMapExtendService, Services.BaseExtend.MapExtendService>();
            services.AddScoped<Services.BaseExtend.Interfaces.INodePositionService, Services.BaseExtend.NodePositionService>();
            services.AddScoped<Services.FlowExtend.Interfaces.ITaskTemplateRuleService, Services.FlowExtend.TaskTemplateRuleService>();
            services.AddScoped<Services.FlowExtend.Interfaces.ITaskTemplateMdcsService, Services.FlowExtend.TaskTemplateMdcsService>();
            services.AddScoped<Services.FlowExtend.Interfaces.ITaskRecordService, Services.FlowExtend.TaskRecordService>();
            services.AddScoped<Services.FlowExtend.Interfaces.ILogisticsRouteService, Services.FlowExtend.LogisticsRouteService>();
            services.AddScoped<Services.RecordExtend.Interfaces.ITrafficService, Services.RecordExtend.TrafficService>();
            services.AddScoped<Services.RecordExtend.Interfaces.IAlarmMdcsService, Services.RecordExtend.AlarmMdcsService>();
            services.AddScoped<Services.RecordExtend.Interfaces.IChargeConsumeService, Services.RecordExtend.ChargeConsumeService>();
            services.AddScoped<Services.RecordExtend.Interfaces.IDisChargeConsumeService, Services.RecordExtend.DisChargeConsumeService>();
            services.AddScoped<Services.Object.Interfaces.IThirdpartySystemService, Services.Object.ThirdpartySystemService>();
            services.AddScoped<Services.Object.Interfaces.IAutoDoorService, Services.Object.AutoDoorService>();
            services.AddScoped<Services.Object.Interfaces.ISafetyLightGridsService, Services.Object.SafetyLightGridsService>();
            services.AddScoped<Services.Object.Interfaces.ISafetyLightGridsItemService, Services.Object.SafetyLightGridsItemService>();
            services.AddScoped<Services.Object.Interfaces.ITrafficLightService, Services.Object.TrafficLightService>();
            services.AddScoped<Services.Object.Interfaces.ITrafficLightItemService, Services.Object.TrafficLightItemService>();
            services.AddScoped<Services.Object.Interfaces.IButtonBoxService, Services.Object.ButtonBoxService>();
            services.AddScoped<Services.Object.Interfaces.IButtonBoxItemService, Services.Object.ButtonBoxItemService>();
            services.AddScoped<Services.Object.Interfaces.IElevatorService, Services.Object.ElevatorService>();
            services.AddScoped<Services.Object.Interfaces.IElevatorItemService, Services.Object.ElevatorItemService>();
            return services;
        }

        public static IServiceProvider UseService(this IServiceProvider provider)
        {
            provider.UseServiceDatabase();
            provider.UseData();
            return provider;
        }

        public static FrameContext? UseServiceDatabase(this IServiceProvider provider)
        {
            var context = provider.UseDataDatabase();

            if (context is null) return context;

            var mapper = provider.GetRequiredService<IMapper>();

            var permissionDtos = new List<PermissionDto>();

            permissionDtos.Add(new PermissionDto()
            {
                ParentId = "Root",
                Type = "Menu",
                Code = "RunManagement",
                Name = "运行管理",
                Icon = "fa-fw fa-solid fa-chart-pie",
                Method = "Get",
                Target = "/RunManagement",
                SortNumber = -105.5
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "RunManagement").Id,
                Type = "Menu",
                Code = "RunManagement-Mission",
                Name = "进程管理",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/RunManagement/Mission/Index",
                SortNumber = 1
            });

            permissionDtos.Add(new PermissionDto()
            {
                ParentId = "Root",
                Type = "Menu",
                Code = "Warehouse",
                Name = "库位配置",
                Icon = "fa-fw fa-solid fa-warehouse",
                Method = "Get",
                Target = "/Warehouse",
                SortNumber = -4
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Warehouse").Id,
                Type = "Menu",
                Code = "Warehouse-Area",
                Name = "库区管理",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Warehouse/Area/Index",
                SortNumber = 1
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Warehouse").Id,
                Type = "Menu",
                Code = "Warehouse-Storage",
                Name = "库位管理",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Warehouse/Storage/Index",
                SortNumber = 2
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Warehouse").Id,
                Type = "Menu",
                Code = "Warehouse-Container",
                Name = "容器管理",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Warehouse/Container/Index",
                SortNumber = 3
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Warehouse").Id,
                Type = "Menu",
                Code = "Warehouse-Material",
                Name = "物料管理",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Warehouse/Material/Index",
                SortNumber = 4
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Warehouse").Id,
                Type = "Menu",
                Code = "Warehouse-Tag",
                Name = "标签管理",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Warehouse/Tag/Index",
                SortNumber = 5
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Warehouse").Id,
                Type = "Menu",
                Code = "Warehouse-StorageContainerHistory",
                Name = "储位容器历史",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Warehouse/StorageContainerHistory/Index",
                SortNumber = 6
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Warehouse").Id,
                Type = "Menu",
                Code = "Warehouse-ContainerMaterialHistory",
                Name = "容器物料历史",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Warehouse/ContainerMaterialHistory/Index",
                SortNumber = 7
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Warehouse").Id,
                Type = "Menu",
                Code = "Warehouse-MaterialStorageHistory",
                Name = "物料储位历史",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Warehouse/MaterialStorageHistory/Index",
                SortNumber = 8
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Warehouse").Id,
                Type = "Menu",
                Code = "Warehouse-Work",
                Name = "任务管理",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Warehouse/Work/Index",
                SortNumber = 9
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Warehouse").Id,
                Type = "Menu",
                Code = "Warehouse-StorageCall",
                Name = "缺料呼叫",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Warehouse/storagecall/Index",
                SortNumber = 10
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Warehouse").Id,
                Type = "Menu",
                Code = "Warehouse-Visualization",
                Name = "库位可视化",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Warehouse/Visualization/Index",
                SortNumber = 5.5
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Warehouse").Id,
                Type = "Menu",
                Code = "Warehouse-VisualizationTask",
                Name = "库位任务可视化",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Warehouse/VisualizationTask/Index",
                SortNumber = 5.6
            });


            permissionDtos.Add(new PermissionDto()
            {
                ParentId = context.Set<PermissionEntity>().First(e => e.Code == "Setting").Id,
                Type = "Menu",
                Code = "Setting-ConfigService",
                Name = "服务配置",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Setting/ConfigService/Index",
                SortNumber = 2
            });

            permissionDtos.Add(new PermissionDto()
            {
                ParentId = "Root",
                Type = "Menu",
                Code = "Signal",
                Name = "实时信号",
                Icon = "fa-fw fa-solid fa-chart-pie",
                Method = "Get",
                Target = "/Signal",
                SortNumber = -2
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Signal").Id,
                Type = "Menu",
                Code = "Safety-Sign",
                Name = "安全信号对接",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Signal/Safety-Sign/Index",
                SortNumber = 1
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Signal").Id,
                Type = "Menu",
                Code = "Signal-GeneralSafetySignal",
                Name = "安全信号",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Signal/GeneralSafetySignal/Index",
                SortNumber = 2
            });
            //permissionDtos.Add(new PermissionDto()
            //{
            //    ParentId = permissionDtos.First(e => e.Code == "Signal").Id,
            //    Type = "Menu",
            //    Code = "Instrument",
            //    Name = "仪表盘",
            //    Icon = "fa-fw fa-solid fa-caret-right",
            //    Method = "Get",
            //    Target = "/Signal/Instrument/Index",
            //    SortNumber = 2
            //});
            //permissionDtos.Add(new PermissionDto()
            //{
            //    ParentId = permissionDtos.First(e => e.Code == "Signal").Id,
            //    Type = "Menu",
            //    Code = "Jump-Record",
            //    Name = "跳变记录",
            //    Icon = "fa-fw fa-solid fa-caret-right",
            //    Method = "Get",
            //    Target = "/Signal/JumpRecord/Index",
            //    SortNumber = 2
            //});
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = "Root",
                Type = "Menu",
                Code = "Report",
                Name = "报表统计",
                Icon = "fa-fw fa-solid fa-chart-pie",
                Method = "Get",
                Target = "/Report",
                SortNumber = -2
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Report").Id,
                Type = "Menu",
                Code = "Report-Home",
                Name = "报表首页",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Report/Home/Index",
                SortNumber = 1
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Report").Id,
                Type = "Menu",
                Code = "Scada",
                Name = "能耗统计",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Report/Scada/Index",
                SortNumber = 1.1
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Report").Id,
                Type = "Menu",
                Code = "Report-Efficiency",
                Name = "效率报表",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Report/Efficiency/Index",
                SortNumber = 2
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Report").Id,
                Type = "Menu",
                Code = "Report-Load",
                Name = "负荷报表",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Report/Load/Index",
                SortNumber = 3
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Report").Id,
                Type = "Menu",
                Code = "Report-Fault",
                Name = "故障报表",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Report/Fault/Index",
                SortNumber = 4
            });

            permissionDtos.Add(new PermissionDto()
            {
                ParentId = "Root",
                Type = "Menu",
                Code = "Mobile",
                Name = "移动管理",
                Icon = "fa-fw fa-solid fa-mobile-alt",
                Method = "Get",
                Target = "/Mobile",
                SortNumber = -2
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Mobile").Id,
                Type = "Menu",
                Code = "Mobile-Home",
                Name = "移动首页",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Mobile/Home/Index",
                SortNumber = 1
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Mobile").Id,
                Type = "Menu",
                Code = "Mobile-Menu",
                Name = "首页菜单",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Mobile/Menu/Index",
                SortNumber = 2
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Mobile").Id,
                Type = "Menu",
                Code = "Mobile-FlowCarTaskSingle",
                Name = "单点任务",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Mobile/FlowCarTaskSingle/Index",
                SortNumber = 3
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Mobile").Id,
                Type = "Menu",
                Code = "Mobile-FlowCarTaskDouble",
                Name = "双点任务",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Mobile/FlowCarTaskDouble/Index",
                SortNumber = 4
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Mobile").Id,
                Type = "Menu",
                Code = "Mobile-FlowCarTaskTemplate",
                Name = "模板任务",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Mobile/FlowCarTaskTemplate/Index",
                SortNumber = 5
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Mobile").Id,
                Type = "Menu",
                Code = "Mobile-WarehouseDataStorage",
                Name = "储位数据",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Mobile/WarehouseDataStorage/Index",
                SortNumber = 6
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Mobile").Id,
                Type = "Menu",
                Code = "Mobile-WarehouseDataContainer",
                Name = "容器数据",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Mobile/WarehouseDataContainer/Index",
                SortNumber = 7
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Mobile").Id,
                Type = "Menu",
                Code = "Mobile-WarehouseDataMaterial",
                Name = "物料数据",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Mobile/WarehouseDataMaterial/Index",
                SortNumber = 8
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Mobile").Id,
                Type = "Menu",
                Code = "Mobile-WarehouseWorkAreaA",
                Name = "A区任务",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Mobile/WarehouseWorkAreaA/Index",
                SortNumber = 9
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Mobile").Id,
                Type = "Menu",
                Code = "Mobile-WarehouseWorkAreaB",
                Name = "B区任务",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Mobile/WarehouseWorkAreaB/Index",
                SortNumber = 10
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Mobile").Id,
                Type = "Menu",
                Code = "Mobile-WarehouseWorkAreaC",
                Name = "C区任务",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Mobile/WarehouseWorkAreaC/Index",
                SortNumber = 11
            });

            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Mobile").Id,
                Type = "Menu",
                Code = "Mobile-StorageCallList",
                Name = "呼叫记录",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Mobile/StorageCallList/Index",
                SortNumber = 12
            });

            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Mobile").Id,
                Type = "Menu",
                Code = "Mobile-StorageCall",
                Name = "缺料呼叫",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Mobile/StorageCall/Index",
                SortNumber = 13
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Mobile").Id,
                Type = "Menu",
                Code = "Mobile-PadCall",
                Name = "平板呼叫",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Mobile/PadCall/Index",
                SortNumber = 13.1
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Mobile").Id,
                Type = "Menu",
                Code = "Mobile-CustomizedPadCall",
                Name = "定制化平板呼叫",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Mobile/CustomizedPadCall/Index",
                SortNumber = 13.2
            });


            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Mobile").Id,
                Type = "Menu",
                Code = "Mobile-MaterialAdd",
                Name = "物料登记",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Mobile/MaterialAdd/Index",
                SortNumber = 13.5
            });

            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Mobile").Id,
                Type = "Menu",
                Code = "Mobile-Runtime",
                Name = "移动运行监控",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Mobile/Runtime/Index",
                SortNumber = 14
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Mobile").Id,
                Type = "Menu",
                Code = "Mobile-Visualization",
                Name = "移动库位可视化",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Mobile/Visualization/Index",
                SortNumber = 15
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Mobile").Id,
                Type = "Menu",
                Code = "Mobile-PreMaterial",
                Name = "预定物料信息",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Mobile/PreMaterial/Index",
                SortNumber = 16
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Mobile").Id,
                Type = "Menu",
                Code = "Mobile-PreWork",
                Name = "物料预定记录",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Mobile/PreWork/Index",
                SortNumber = 17
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = "Root",
                Type = "Menu",
                Code = "Screen",
                Name = "大屏管理",
                Icon = "fa-fw fa-solid fa-desktop",
                Method = "Get",
                Target = "/Screen",
                SortNumber = -1
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Screen").Id,
                Type = "Menu",
                Code = "Screen-Home",
                Name = "大屏首页",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Screen/Home/Index",
                SortNumber = 1
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Screen").Id,
                Type = "Menu",
                Code = "Screen-Data",
                Name = "数据展示",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Screen/Data/Index",
                SortNumber = 2
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Screen").Id,
                Type = "Menu",
                Code = "Screen-Runtime",
                Name = "运行监控",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Screen/Runtime/Index",
                SortNumber = 3
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Screen").Id,
                Type = "Menu",
                Code = "Screen-Map",
                Name = "地图监控",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Screen/Map/Index",
                SortNumber = 4
            });

            permissionDtos.Add(new PermissionDto()
            {
                ParentId = context.Set<PermissionEntity>().First(e => e.Code == "Data").Id,
                Type = "Menu",
                Code = "Data-Envelope",
                Name = "包络配置",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Data/Envelope/Index",
                SortNumber = 8.0
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = context.Set<PermissionEntity>().First(e => e.Code == "Data").Id,
                Type = "Menu",
                Code = "Data-TrafficRules",
                Name = "交管配置",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Data/TrafficRules/Index",
                SortNumber = 9.0
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = context.Set<PermissionEntity>().First(e => e.Code == "Data").Id,
                Type = "Menu",
                Code = "Data-PlanRules",
                Name = "路径规划配置",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Data/PlanRules/Index",
                SortNumber = 10.0
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = context.Set<PermissionEntity>().First(e => e.Code == "Data").Id,
                Type = "Menu",
                Code = "Data-Attribute",
                Name = "属性配置",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Data/Attribute/Index",
                SortNumber = 11.0
            });


            permissionDtos.Add(new PermissionDto()
            {
                ParentId = context.Set<PermissionEntity>().First(e => e.Code == "Record").Id,
                Type = "Menu",
                Code = "Record-PlcRawData",
                Name = "任务呼叫记录",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Record/PlcRawData/Index",
                SortNumber = 3.0
            });


            permissionDtos.Add(new PermissionDto()
            {
                ParentId = "Root",
                Type = "Menu",
                Code = "Task",
                Name = "任务管理",
                Icon = "fa-solid:clipboard-list",
                Method = "Get",
                Target = "/Task",
                SortNumber = -100.1
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Task").Id,
                Type = "Menu",
                Code = "Task-TaskRecord",
                Name = "任务记录",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Task/TaskRecord/Index",
                SortNumber = 1
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Task").Id,
                Type = "Menu",
                Code = "Task-TaskTemplateMdcs",
                Name = "任务模板",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Task/TaskTemplateMdcs/Index",
                SortNumber = 2
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Task").Id,
                Type = "Menu",
                Code = "Task-LogisticsRoute",
                Name = "物流动线",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Task/LogisticsRoute/Index",
                SortNumber = 3
            });

            permissionDtos.Add(new PermissionDto()
            {
                ParentId = "Root",
                Type = "Menu",
                Code = "Object",
                Name = "交互对象",
                Icon = "fa-solid:shapes",
                Method = "Get",
                Target = "/Object",
                SortNumber = -100.5
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Object").Id,
                Type = "Menu",
                Code = "Object-Charging",
                Name = "充电桩",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Object/Charging/Index",
                SortNumber = 1
            });

            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Object").Id,
                Type = "Menu",
                Code = "Object-Elevator",
                Name = "电梯",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Object/Elevator/Index",
                SortNumber = 3
            });

            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Object").Id,
                Type = "Menu",
                Code = "Object-ButtonBox",
                Name = "按钮盒",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Object/ButtonBox/Index",
                SortNumber = 5
            });

            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Object").Id,
                Type = "Menu",
                Code = "Object-SafetyLightGrid",
                Name = "安全光栅",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Object/SafetyLightGrid/Index",
                SortNumber = 6
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Object").Id,
                Type = "Menu",
                Code = "Object-TrafficLight",
                Name = "红绿灯",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Object/TrafficLight/Index",
                SortNumber = 7
            });
            permissionDtos.Add(new PermissionDto()
            {
                ParentId = permissionDtos.First(e => e.Code == "Object").Id,
                Type = "Menu",
                Code = "Object-ThirdPartySystem",
                Name = "第三方系统",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Object/ThirdPartySystem/Index",
                SortNumber = 8
            });

            permissionDtos.Add(new PermissionDto
            {
                ParentId = context.Set<PermissionEntity>().First(e => e.Code == "Monitor").Id,
                Type = "Menu",
                Code = "Monitor-Traffic",
                Name = "交管监控",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Monitor/Traffic/Index",
                SortNumber = 4.0
            });
            permissionDtos.Add(new PermissionDto
            {
                ParentId = context.Set<PermissionEntity>().First(e => e.Code == "Monitor").Id,
                Type = "Menu",
                Code = "Monitor-AlarmMdcs",
                Name = "报警监控",
                Icon = "fa-fw fa-solid fa-caret-right",
                Method = "Get",
                Target = "/Monitor/AlarmMdcs/Index",
                SortNumber = 5.0
            });


            var permissionEntities = mapper.Map<List<PermissionEntity>>(permissionDtos);
            context.AddRange(permissionEntities);

            var userDtos = new List<UserDto>();

            userDtos.Add(new UserDto()
            {
                Id = "Mobile",
                Username = "Mobile",
                Password = SecurityHelper.HashSHA256("123456"),
                Name = "移动",
                Nick = "移动",
                Gender = "Unknown",
                Birthday = new DateTime(1988, 8, 8),
                Phone = "18888888888",
                Mail = "mobile@mail.com",
                Avatar = "",
                IsOnline = false,
                IsSystem = false
            });

            userDtos.Add(new UserDto()
            {
                Id = "Screen",
                Username = "Screen",
                Password = SecurityHelper.HashSHA256("123456"),
                Name = "大屏",
                Nick = "大屏",
                Gender = "Unknown",
                Birthday = new DateTime(1988, 8, 8),
                Phone = "18888888888",
                Mail = "screen@mail.com",
                Avatar = "",
                IsOnline = false,
                IsSystem = false
            });

            var userEntities = mapper.Map<List<UserEntity>>(userDtos);
            context.AddRange(userEntities);

            var roleDtos = new List<RoleDto>();

            roleDtos.Add(new RoleDto()
            {
                Id = "Mobile",
                Code = "Mobile",
                Name = "移动"
            });

            roleDtos.Add(new RoleDto()
            {
                Id = "Screen",
                Code = "Screen",
                Name = "大屏"
            });

            var roleEntities = mapper.Map<List<RoleEntity>>(roleDtos);
            context.AddRange(roleEntities);

            var userRoleDtos = new List<UserRoleDto>();

            userRoleDtos.Add(new UserRoleDto()
            {
                Id = "Mobile",
                UserId = "Mobile",
                RoleId = "Mobile"
            });

            userRoleDtos.Add(new UserRoleDto()
            {
                Id = "Screen",
                UserId = "Screen",
                RoleId = "Screen"
            });

            var userRoleEntities = mapper.Map<List<UserRoleEntity>>(userRoleDtos);
            context.AddRange(userRoleEntities);

            var rolePermissionDtos = new List<RolePermissionDto>();

            rolePermissionDtos.AddRange(permissionDtos.Where(e => e.Code.StartsWith("Mobile")).Select(e => new RolePermissionDto()
            {
                RoleId = "Mobile",
                PermissionId = e.Id
            }));

            rolePermissionDtos.AddRange(permissionDtos.Where(e => e.Code.StartsWith("Screen")).Select(e => new RolePermissionDto()
            {
                RoleId = "Screen",
                PermissionId = e.Id
            }));

            var rolePermissionEntities = mapper.Map<List<RolePermissionEntity>>(rolePermissionDtos);
            context.AddRange(rolePermissionEntities);

            var configDtos = new List<ConfigDto>();

            configDtos.Add(new ConfigDto()
            {
                Key = "Item1",
                Value = "100"
            });
            configDtos.Add(new ConfigDto()
            {
                Key = "Item2",
                Value = "200"
            });
            configDtos.Add(new ConfigDto()
            {
                Key = "Item3",
                Value = "300"
            });

            configDtos.Add(new ConfigDto()
            {
                Key = "TaskAvailableBattery",
                Value = "65"
            });
            configDtos.Add(new ConfigDto()
            {
                Key = "CarIdleChargeBattery",
                Value = "65"
            });

            configDtos.Add(new ConfigDto()
            {
                Key = "AlarmTriggerDuration",
                Value = "2000"
            });

            configDtos.Add(new ConfigDto()
            {
                Key = "RecyclingArea",
                Value = "{\"fullArea\":\"MTZZQ\",\"emptyArea\":\"KTZZQ\"}"
            });

            var configEntities = mapper.Map<List<ConfigEntity>>(configDtos);
            context.AddRange(configEntities);

            var dictDtos = new List<DictDto>();
            var dictItemDtos = new List<DictItemDto>();

            dictDtos.Add(new DictDto()
            {
                Code = "TaskType",
                Name = "任务业务类型"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "TaskType").Id,
                Code = "Template",
                Name = "模板任务",
                SortNumber = 1
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "TaskType").Id,
                Code = "LogisticsRoute",
                Name = "物流动线",
                SortNumber = 2
            });

            dictDtos.Add(new DictDto()
            {
                Code = "CallMode",
                Name = "呼叫模式"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CallMode").Id,
                Code = "FullOnline",
                Name = "满桶上线",
                Param = "HG/DS",
                SortNumber = 1
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CallMode").Id,
                Code = "EmptyOffline",
                Name = "空桶下线",
                Param = "HG/DS",
                SortNumber = 2
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CallMode").Id,
                Code = "EmptyFullExchange",
                Name = "空桶下线/满桶上线",
                //Param = "HG/DS",
                SortNumber = 3
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CallMode").Id,
                Code = "FullOffline",
                Name = "满桶下线",
                Param = "LX",
                SortNumber = 4
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CallMode").Id,
                Code = "EmptyOnline",
                Name = "空桶上线",
                Param = "LX",
                SortNumber = 5
            });

            dictDtos.Add(new DictDto()
            {
                Code = "CommonCallMode",
                Name = "通用呼叫模板"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CommonCallMode").Id,
                Code = "FullOnline",
                Name = "送满",
                SortNumber = 1
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CommonCallMode").Id,
                Code = "FullOffline",
                Name = "取满",
                SortNumber = 2
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CommonCallMode").Id,
                Code = "EmptyOffline",
                Name = "取空",
                SortNumber = 3
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CommonCallMode").Id,
                Code = "EmptyOnline",
                Name = "送空",
                SortNumber = 4
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CommonCallMode").Id,
                Code = "EmptyFullExchange",
                Name = "取空放满",
                SortNumber = 5
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CommonCallMode").Id,
                Code = "FullEmptyExchange",
                Name = "取满放空",
                SortNumber = 6
            });

            dictDtos.Add(new DictDto()
            {
                Code = "AreaType",
                Name = "区域类型"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "AreaType").Id,
                Code = "Default",
                Name = "默认",
                SortNumber = 1
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "AreaType").Id,
                Code = "PdaCall",
                Name = "PDA叫料区域",
                SortNumber = 2
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "AreaType").Id,
                Code = "PlcCall",
                Name = "PLC叫料区域",
                SortNumber = 3
            });

            dictDtos.Add(new DictDto()
            {
                Code = "AreaState",
                Name = "区域状态"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "AreaState").Id,
                Code = "Default",
                Name = "默认",
                SortNumber = 1
            });

            dictDtos.Add(new DictDto()
            {
                Code = "StorageType",
                Name = "储位类型"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "StorageType").Id,
                Code = "Default",
                Name = "默认",
                SortNumber = 1
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "StorageType").Id,
                Code = "Fetch",
                Name = "下料位",
                SortNumber = 2,
                Param = "Pad"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "StorageType").Id,
                Code = "Put",
                Name = "上料位",
                SortNumber = 3,
                Param = "Pad"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "StorageType").Id,
                Code = "Call",
                Name = "呼叫位",
                SortNumber = 4,
                Param = "Pad"
            });

            dictDtos.Add(new DictDto()
            {
                Code = "StorageState",
                Name = "储位状态"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "StorageState").Id,
                Code = "NoneContainer",
                Name = "无容器",
                SortNumber = 1
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "StorageState").Id,
                Code = "EmptyContainer",
                Name = "空容器",
                SortNumber = 2
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "StorageState").Id,
                Code = "FullContainer",
                Name = "满容器",
                SortNumber = 3
            });

            dictDtos.Add(new DictDto()
            {
                Code = "ContainerType",
                Name = "容器类型"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "ContainerType").Id,
                Code = "Default",
                Name = "默认",
                SortNumber = 1
            });

            dictDtos.Add(new DictDto()
            {
                Code = "ContainerState",
                Name = "容器状态"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "ContainerState").Id,
                Code = "EmptyMaterial",
                Name = "空物料",
                SortNumber = 1
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "ContainerState").Id,
                Code = "FullMaterial",
                Name = "满物料",
                SortNumber = 2
            });

            dictDtos.Add(new DictDto()
            {
                Code = "MaterialType",
                Name = "物料类型"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "MaterialType").Id,
                Code = "Default",
                Name = "默认",
                SortNumber = 1
            });

            dictDtos.Add(new DictDto()
            {
                Code = "MaterialState",
                Name = "物料状态"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "MaterialState").Id,
                Code = "Bind",
                Name = "绑定",
                SortNumber = 1
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "MaterialState").Id,
                Code = "UnBind",
                Name = "解绑",
                SortNumber = 2
            });

            dictDtos.Add(new DictDto()
            {
                Code = "StorageContainerHistoryState",
                Name = "储位容器历史状态"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "StorageContainerHistoryState").Id,
                Code = "Add",
                Name = "添加",
                SortNumber = 1
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "StorageContainerHistoryState").Id,
                Code = "Delete",
                Name = "删除",
                SortNumber = 2
            });

            dictDtos.Add(new DictDto()
            {
                Code = "ContainerMaterialHistoryState",
                Name = "容器物料历史状态"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "ContainerMaterialHistoryState").Id,
                Code = "Add",
                Name = "添加",
                SortNumber = 1
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "ContainerMaterialHistoryState").Id,
                Code = "Delete",
                Name = "删除",
                SortNumber = 2
            });

            dictDtos.Add(new DictDto()
            {
                Code = "MaterialStorageHistoryState",
                Name = "物料储位历史状态"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "MaterialStorageHistoryState").Id,
                Code = "Add",
                Name = "添加",
                SortNumber = 1
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "MaterialStorageHistoryState").Id,
                Code = "Delete",
                Name = "删除",
                SortNumber = 2
            });

            dictDtos.Add(new DictDto()
            {
                Code = "WorkType",
                Name = "任务类型"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "WorkType").Id,
                Code = "Normal",
                Name = "默认",
                SortNumber = 1
            });

            dictDtos.Add(new DictDto()
            {
                Code = "WorkState",
                Name = "任务状态"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "WorkState").Id,
                Code = "Created",
                Name = "创建",
                SortNumber = 1
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "WorkState").Id,
                Code = "Released",
                Name = "发布",
                SortNumber = 2
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "WorkState").Id,
                Code = "Distributed",
                Name = "分发",
                SortNumber = 3
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "WorkState").Id,
                Code = "Running",
                Name = "运行中",
                SortNumber = 4
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "WorkState").Id,
                Code = "Pausing",
                Name = "暂停中",
                SortNumber = 5
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "WorkState").Id,
                Code = "Paused",
                Name = "暂停",
                SortNumber = 6
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "WorkState").Id,
                Code = "Resuming",
                Name = "恢复中",
                SortNumber = 7
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "WorkState").Id,
                Code = "Resumed",
                Name = "恢复",
                SortNumber = 8
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "WorkState").Id,
                Code = "Completing",
                Name = "完成中",
                SortNumber = 9
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "WorkState").Id,
                Code = "Canceling",
                Name = "取消中",
                SortNumber = 10
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "WorkState").Id,
                Code = "Faulting",
                Name = "失败中",
                SortNumber = 11
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "WorkState").Id,
                Code = "Completed",
                Name = "完成",
                SortNumber = 12
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "WorkState").Id,
                Code = "Canceled",
                Name = "取消",
                SortNumber = 13
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "WorkState").Id,
                Code = "Faulted",
                Name = "失败",
                SortNumber = 14
            });

            dictDtos.Add(new DictDto()
            {
                Code = "PreWorkState",
                Name = "预定任务状态"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "PreWorkState").Id,
                Code = "Created",
                Name = "创建",
                SortNumber = 1
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "PreWorkState").Id,
                Code = "Released",
                Name = "发布",
                SortNumber = 2
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "PreWorkState").Id,
                Code = "Completed",
                Name = "完成",
                SortNumber = 3
            });

            dictDtos.Add(new DictDto()
            {
                Code = "TaskRecordState",
                Name = "任务记录状态"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "TaskRecordState").Id,
                Code = "Created",
                Name = "创建",
                SortNumber = 1
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "TaskRecordState").Id,
                Code = "Released",
                Name = "发布",
                SortNumber = 2
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "TaskRecordState").Id,
                Code = "Distributed",
                Name = "分发",
                SortNumber = 3
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "TaskRecordState").Id,
                Code = "Running",
                Name = "运行中",
                SortNumber = 4
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "TaskRecordState").Id,
                Code = "Fetching",
                Name = "取货中",
                SortNumber = 5
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "TaskRecordState").Id,
                Code = "Putting",
                Name = "放货中",
                SortNumber = 6
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "TaskRecordState").Id,
                Code = "Pausing",
                Name = "暂停中",
                SortNumber = 7
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "TaskRecordState").Id,
                Code = "Resuming",
                Name = "恢复中",
                SortNumber = 8
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "TaskRecordState").Id,
                Code = "Completed",
                Name = "完成",
                SortNumber = 9
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "TaskRecordState").Id,
                Code = "Canceled",
                Name = "取消",
                SortNumber = 10
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "TaskRecordState").Id,
                Code = "Faulted",
                Name = "失败",
                SortNumber = 11
            });

            dictDtos.Add(new DictDto()
            {
                Code = "ProtocolType",
                Name = "协议类型"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "ProtocolType").Id,
                Code = "None",
                Name = "未设置",
                SortNumber = 1
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "ProtocolType").Id,
                Code = "Tcp",
                Name = "Tcp",
                SortNumber = 2
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "ProtocolType").Id,
                Code = "Udp",
                Name = "Udp",
                SortNumber = 3
            });

            dictDtos.Add(new DictDto()
            {
                Code = "ChargingMode",
                Name = "充电模式"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "ChargingMode").Id,
                Code = "Side",
                Name = "侧充",
                SortNumber = 1
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "ChargingMode").Id,
                Code = "Tail",
                Name = "尾充",
                SortNumber = 2
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "ChargingMode").Id,
                Code = "Bottom",
                Name = "地充",
                SortNumber = 3
            });

            dictDtos.Add(new DictDto()
            {
                Code = "ConnectState",
                Name = "在线状态"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "ConnectState").Id,
                Code = "Online",
                Name = "在线",
                SortNumber = 1
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "ConnectState").Id,
                Code = "Offline",
                Name = "离线",
                SortNumber = 2
            });

            dictDtos.Add(new DictDto()
            {
                Code = "CarProtocolType",
                Name = "车辆协议类型"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarProtocolType").Id,
                Code = "Fairyland.Plc",
                Name = "Plc",
                SortNumber = 1
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarProtocolType").Id,
                Code = "Fairyland.Pcb",
                Name = "Pcb",
                SortNumber = 2
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarProtocolType").Id,
                Code = "Fairyland.Pc",
                Name = "Pc",
                SortNumber = 3
            });

            dictDtos.Add(new DictDto()
            {
                Code = "CarState",
                Name = "车辆状态"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarState").Id,
                Code = "None",
                Name = "未设置",
                Value = 0,
                SortNumber = 1
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarState").Id,
                Code = "Running",
                Name = "运行中",
                Value = 1,
                SortNumber = 2
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarState").Id,
                Code = "Stopping",
                Name = "停止中",
                Value = 2,
                SortNumber = 3
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarState").Id,
                Code = "EmergencyStop",
                Name = "急停中",
                Value = 3,
                SortNumber = 4
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarState").Id,
                Code = "Faulting",
                Name = "故障中",
                Value = 4,
                SortNumber = 5
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarState").Id,
                Code = "Tasking",
                Name = "任务中",
                Value = 5,
                SortNumber = 6
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarState").Id,
                Code = "Dormancying",
                Name = "休眠中",
                Value = 6,
                SortNumber = 7
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarState").Id,
                Code = "Shutdown",
                Name = "关机中",
                Value = 7,
                SortNumber = 8
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarState").Id,
                Code = "Charging",
                Name = "充电中",
                Value = 8,
                SortNumber = 9
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarState").Id,
                Code = "StopAccept",
                Name = "停接任务中",
                Value = 20,
                SortNumber = 21
            });

            dictDtos.Add(new DictDto()
            {
                Code = "CarAlarm",
                Name = "车辆报警"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "0",
                Name = "急停",
                Value = 0,
                Param = "2",
                SortNumber = 1
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "1",
                Name = "网络异常",
                Value = 1,
                Param = "0",
                SortNumber = 2
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "2",
                Name = "脱轨异常报警",
                Value = 2,
                Param = "2",
                SortNumber = 3
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "3",
                Name = "障碍物异常报警",
                Value = 3,
                Param = "0",
                SortNumber = 4
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "4",
                Name = "安全触边报警",
                Value = 4,
                Param = "2",
                SortNumber = 5
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "5",
                Name = "驱动报警",
                Value = 5,
                Param = "2",
                SortNumber = 6
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "6",
                Name = "低电压报警",
                Value = 6,
                Param = "2",
                SortNumber = 7
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "7",
                Name = "低电量报警",
                Value = 7,
                Param = "0",
                SortNumber = 8
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "8",
                Name = "1#驱动器故障",
                Value = 8,
                Param = "2",
                SortNumber = 9
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "9",
                Name = "2#驱动器故障",
                Value = 9,
                Param = "2",
                SortNumber = 10
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "10",
                Name = "3#驱动器故障",
                Value = 10,
                Param = "2",
                SortNumber = 11
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "11",
                Name = "4#驱动器故障",
                Value = 11,
                Param = "2",
                SortNumber = 12
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "12",
                Name = "急停2",
                Value = 12,
                Param = "2",
                SortNumber = 13
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "13",
                Name = "脱轨异常2",
                Value = 13,
                Param = "2",
                SortNumber = 14
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "14",
                Name = "安全触边2报警",
                Value = 14,
                Param = "2",
                SortNumber = 15
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "15",
                Name = "1#驱动器通讯异常",
                Value = 15,
                Param = "2",
                SortNumber = 16
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "16",
                Name = "2#驱动器通讯异常",
                Value = 16,
                Param = "2",
                SortNumber = 17
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "17",
                Name = "3#驱动器通讯异常",
                Value = 17,
                Param = "2",
                SortNumber = 18
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "18",
                Name = "4#驱动器通讯异常",
                Value = 18,
                Param = "2",
                SortNumber = 19
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "19",
                Name = "RFID通讯异常",
                Value = 19,
                Param = "2",
                SortNumber = 20
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "20",
                Name = "1#障碍物检测减速停止区报警",
                Value = 20,
                Param = "1",
                SortNumber = 21
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "21",
                Name = "1#障碍物检测急停区报警",
                Value = 21,
                Param = "2",
                SortNumber = 22
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "22",
                Name = "2#障碍物检测减速停止区报警",
                Value = 22,
                Param = "1",
                SortNumber = 23
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "23",
                Name = "2#障碍物检测急停区报警",
                Value = 23,
                Param = "2",
                SortNumber = 24
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "24",
                Name = "1#牵引棒上升超时报警",
                Value = 24,
                Param = "2",
                SortNumber = 25
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "25",
                Name = "1#牵引棒下降超时报警",
                Value = 25,
                Param = "2",
                SortNumber = 26
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "26",
                Name = "1#牵引棒传感器检测异常",
                Value = 26,
                Param = "2",
                SortNumber = 27
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "27",
                Name = "2#牵引棒上升超时报警",
                Value = 27,
                Param = "2",
                SortNumber = 28
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "28",
                Name = "2#牵引棒下降超时报警",
                Value = 28,
                Param = "2",
                SortNumber = 29
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "29",
                Name = "2#牵引棒传感器检测异常",
                Value = 29,
                Param = "2",
                SortNumber = 30
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "30",
                Name = "低电量预警",
                Value = 30,
                Param = "0",
                SortNumber = 31
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "31",
                Name = "地标读取错误报警",
                Value = 31,
                Param = "2",
                SortNumber = 32
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "32",
                Name = "电池通讯异常",
                Value = 32,
                Param = "2",
                SortNumber = 33
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "33",
                Name = "磁导航传感器通讯异常",
                Value = 33,
                Param = "2",
                SortNumber = 34
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "34",
                Name = "RFID漏读报警",
                Value = 34,
                Param = "2",
                SortNumber = 35
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "100",
                Name = "调度失联告警",
                Value = 100,
                Param = "1",
                SortNumber = 101
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "101",
                Name = "网络不通",
                Value = 101,
                Param = "1",
                SortNumber = 102
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "102",
                Name = "设备重启",
                Value = 102,
                Param = "0",
                SortNumber = 103
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "103",
                Name = "CPU占用过高",
                Value = 103,
                Param = "1",
                SortNumber = 104
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "104",
                Name = "激光导航定位丢失",
                Value = 104,
                Param = "2",
                SortNumber = 105
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "105",
                Name = "地纹导航脱轨",
                Value = 105,
                Param = "2",
                SortNumber = 106
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "106",
                Name = "二维码导航脱轨",
                Value = 106,
                Param = "2",
                SortNumber = 107
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "107",
                Name = "速度跟随异常",
                Value = 107,
                Param = "2",
                SortNumber = 108
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "108",
                Name = "打滑异常",
                Value = 108,
                Param = "2",
                SortNumber = 109
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "109",
                Name = "举升超时未达到目标",
                Value = 109,
                Param = "2",
                SortNumber = 110
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "110",
                Name = "举升机构脱离目标位置",
                Value = 110,
                Param = "2",
                SortNumber = 111
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "111",
                Name = "低电压预警",
                Value = 111,
                Param = "0",
                SortNumber = 112
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "112",
                Name = "举升动作执行失败",
                Value = 112,
                Param = "2",
                SortNumber = 113
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "113",
                Name = "激光避障关闭",
                Value = 113,
                Param = "0",
                SortNumber = 114
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "114",
                Name = "旋转动作执行失败",
                Value = 114,
                Param = "2",
                SortNumber = 115
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "115",
                Name = "前碰撞条触发停止机器人",
                Value = 115,
                Param = "2",
                SortNumber = 116
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "116",
                Name = "左碰撞条触发停止机器人",
                Value = 116,
                Param = "2",
                SortNumber = 117
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "117",
                Name = "右碰撞条触发停止机器人",
                Value = 117,
                Param = "2",
                SortNumber = 118
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "118",
                Name = "后碰撞条触发停止机器人",
                Value = 118,
                Param = "2",
                SortNumber = 119
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "119",
                Name = "二维码二次调节超时",
                Value = 119,
                Param = "2",
                SortNumber = 120
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "120",
                Name = "下相机通信失败",
                Value = 120,
                Param = "2",
                SortNumber = 121
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "121",
                Name = "上相机通信失败",
                Value = 121,
                Param = "2",
                SortNumber = 122
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "122",
                Name = "二次调节误差过大告警",
                Value = 122,
                Param = "2",
                SortNumber = 123
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "123",
                Name = "找码失败",
                Value = 123,
                Param = "2",
                SortNumber = 124
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "124",
                Name = "避障相机通讯超时报警",
                Value = 124,
                Param = "2",
                SortNumber = 125
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "125",
                Name = "检测相机通讯超时报警",
                Value = 125,
                Param = "2",
                SortNumber = 126
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "126",
                Name = "导航雷达通讯超时报警",
                Value = 126,
                Param = "2",
                SortNumber = 127
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "127",
                Name = "避障雷达通讯超时报警",
                Value = 127,
                Param = "2",
                SortNumber = 128
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "128",
                Name = "检测雷达通讯超时报警",
                Value = 128,
                Param = "2",
                SortNumber = 129
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "129",
                Name = "上相机通讯超时报警",
                Value = 129,
                Param = "2",
                SortNumber = 130
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "130",
                Name = "下相机通讯超时报警",
                Value = 130,
                Param = "2",
                SortNumber = 131
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "131",
                Name = "运动误差过大报警",
                Value = 131,
                Param = "2",
                SortNumber = 132
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "132",
                Name = "地图路径段速度配置错误",
                Value = 132,
                Param = "2",
                SortNumber = 133
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "133",
                Name = "同步举升位置偏差过大",
                Value = 133,
                Param = "2",
                SortNumber = 134
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "134",
                Name = "下放未到位",
                Value = 134,
                Param = "2",
                SortNumber = 135
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "135",
                Name = "上举未到位",
                Value = 135,
                Param = "2",
                SortNumber = 136
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "136",
                Name = "旋转动作超时",
                Value = 136,
                Param = "2",
                SortNumber = 137
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "137",
                Name = "旋转归零时零位未找到",
                Value = 137,
                Param = "2",
                SortNumber = 138
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "138",
                Name = "转盘初始化超时",
                Value = 138,
                Param = "2",
                SortNumber = 139
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "139",
                Name = "举升超时未达到目标",
                Value = 139,
                Param = "2",
                SortNumber = 140
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "140",
                Name = "左电机电流异常",
                Value = 140,
                Param = "2",
                SortNumber = 141
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "141",
                Name = "右电机电流异常",
                Value = 141,
                Param = "2",
                SortNumber = 142
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "142",
                Name = "旋转电机电流异常",
                Value = 142,
                Param = "2",
                SortNumber = 143
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "143",
                Name = "举升电机电流异常",
                Value = 143,
                Param = "2",
                SortNumber = 144
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "144",
                Name = "滚筒电机电流异常",
                Value = 144,
                Param = "2",
                SortNumber = 145
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "145",
                Name = "左电机异常",
                Value = 145,
                Param = "2",
                SortNumber = 146
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "146",
                Name = "右电机异常",
                Value = 146,
                Param = "2",
                SortNumber = 147
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "147",
                Name = "前舵轮电机异常",
                Value = 147,
                Param = "2",
                SortNumber = 148
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "148",
                Name = "后舵轮电机异常",
                Value = 148,
                Param = "2",
                SortNumber = 149
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "149",
                Name = "前转向电机异常",
                Value = 149,
                Param = "2",
                SortNumber = 150
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "150",
                Name = "后转向电机异常",
                Value = 150,
                Param = "2",
                SortNumber = 151
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "151",
                Name = "旋转电机异常",
                Value = 151,
                Param = "2",
                SortNumber = 152
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "152",
                Name = "举升电机异常",
                Value = 152,
                Param = "2",
                SortNumber = 153
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "153",
                Name = "平移电机异常",
                Value = 153,
                Param = "2",
                SortNumber = 154
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "154",
                Name = "滚筒电机异常",
                Value = 154,
                Param = "2",
                SortNumber = 155
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "155",
                Name = "背货未识别",
                Value = 155,
                Param = "2",
                SortNumber = 156
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "156",
                Name = "无激光数据",
                Value = 156,
                Param = "2",
                SortNumber = 157
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "157",
                Name = "左电机速度跟随异常",
                Value = 157,
                Param = "2",
                SortNumber = 158
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "158",
                Name = "右电机速度跟随异常",
                Value = 158,
                Param = "2",
                SortNumber = 159
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "159",
                Name = "旋转电机速度跟随异常",
                Value = 159,
                Param = "2",
                SortNumber = 160
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "160",
                Name = "举升电机速度跟随异常",
                Value = 160,
                Param = "2",
                SortNumber = 161
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "161",
                Name = "平移电机速度跟随异常",
                Value = 161,
                Param = "2",
                SortNumber = 162
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "162",
                Name = "举升电机CAN异常",
                Value = 162,
                Param = "2",
                SortNumber = 163
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "163",
                Name = "平移电机CAN异常",
                Value = 163,
                Param = "2",
                SortNumber = 164
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "164",
                Name = "旋转电机CAN异常",
                Value = 164,
                Param = "2",
                SortNumber = 165
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "165",
                Name = "左电机CAN异常",
                Value = 165,
                Param = "2",
                SortNumber = 166
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "166",
                Name = "右电机CAN异常",
                Value = 166,
                Param = "2",
                SortNumber = 167
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "167",
                Name = "前舵行走CAN异常",
                Value = 167,
                Param = "2",
                SortNumber = 168
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "168",
                Name = "后舵行走CAN异常",
                Value = 168,
                Param = "2",
                SortNumber = 169
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "169",
                Name = "前舵转向CAN异常",
                Value = 169,
                Param = "2",
                SortNumber = 170
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "170",
                Name = "后舵转向CAN异常",
                Value = 170,
                Param = "2",
                SortNumber = 171
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "171",
                Name = "称重传感器异常",
                Value = 171,
                Param = "2",
                SortNumber = 172
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "172",
                Name = "称重传感器偏载",
                Value = 172,
                Param = "2",
                SortNumber = 173
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "173",
                Name = "称重传感器超重",
                Value = 173,
                Param = "2",
                SortNumber = 174
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "174",
                Name = "左侧叉尖遇障，叉车项目使用",
                Value = 174,
                Param = "2",
                SortNumber = 175
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "175",
                Name = "右侧叉尖遇障，叉车项目使用",
                Value = 175,
                Param = "2",
                SortNumber = 176
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "176",
                Name = "货物检测开关未触发异常",
                Value = 176,
                Param = "2",
                SortNumber = 177
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "177",
                Name = "货物探测-未检测到货物",
                Value = 177,
                Param = "2",
                SortNumber = 178
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "178",
                Name = "放料过程中物料掉落告警",
                Value = 178,
                Param = "2",
                SortNumber = 179
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "179",
                Name = "托盘/库位识别误差过大告警",
                Value = 179,
                Param = "2",
                SortNumber = 180
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "180",
                Name = "托盘/库位识别超时报警",
                Value = 180,
                Param = "2",
                SortNumber = 181
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "181",
                Name = "货物开关误触发",
                Value = 181,
                Param = "2",
                SortNumber = 182
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "182",
                Name = "伸缩动作执行失败",
                Value = 182,
                Param = "2",
                SortNumber = 183
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "183",
                Name = "伸缩叉遇障",
                Value = 183,
                Param = "1",
                SortNumber = 184
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "184",
                Name = "伸缩动作超时机构卡死",
                Value = 184,
                Param = "2",
                SortNumber = 185
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAlarm").Id,
                Code = "185",
                Name = "伸缩归零动作超时机构卡死",
                Value = 185,
                Param = "2",
                SortNumber = 186
            });

            dictDtos.Add(new DictDto()
            {
                Code = "DoorMalfunction",
                Name = "卷帘门故障"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "1",
                Name = "系统过流",
                Value = 1,
                SortNumber = 1
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "3",
                Name = "系统欠压",
                Value = 3,
                SortNumber = 3
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "4",
                Name = "停机时过压",
                Value = 4,
                SortNumber = 4
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "5",
                Name = "运行时过压",
                Value = 5,
                SortNumber = 5
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "6",
                Name = "电机堵转",
                Value = 6,
                SortNumber = 6
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "7",
                Name = "超出限位位置",
                Value = 7,
                SortNumber = 7
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "8",
                Name = "主板数据存储故障",
                Value = 8,
                SortNumber = 8
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "9",
                Name = "超速故障",
                Value = 9,
                SortNumber = 9
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "10",
                Name = "反转故障",
                Value = 10,
                SortNumber = 10
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "11",
                Name = "系统过载",
                Value = 11,
                SortNumber = 11
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "12",
                Name = "电流检测回路故障",
                Value = 12,
                SortNumber = 12
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "13",
                Name = "电机编码器故障",
                Value = 13,
                SortNumber = 13
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "14",
                Name = "电机初始角错误",
                Value = 14,
                SortNumber = 14
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "15",
                Name = "通信故障",
                Value = 15,
                SortNumber = 15
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "16",
                Name = "上电检测",
                Value = 16,
                SortNumber = 16
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "17",
                Name = "掉电检测",
                Value = 17,
                SortNumber = 17
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "18",
                Name = "制动回路故障",
                Value = 18,
                SortNumber = 18
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "19",
                Name = "外置编码器故障",
                Value = 19,
                SortNumber = 19
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "20",
                Name = "运行超时",
                Value = 20,
                SortNumber = 20
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "21",
                Name = "单周期内安全信号1异常",
                Value = 21,
                SortNumber = 21
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "22",
                Name = "单周期内安全信号2异常",
                Value = 22,
                SortNumber = 22
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "23",
                Name = "未进行电子行程设定",
                Value = 23,
                SortNumber = 23
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "24",
                Name = "24V电源短路",
                Value = 24,
                SortNumber = 24
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "26",
                Name = "限位开关故障",
                Value = 26,
                SortNumber = 26
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "27",
                Name = "系统过热",
                Value = 27,
                SortNumber = 27
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "28",
                Name = "电磁制动器故障",
                Value = 28,
                SortNumber = 28
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "29",
                Name = "绝对值编码器复位",
                Value = 29,
                SortNumber = 29
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "30",
                Name = "电机参数匹配故障",
                Value = 30,
                SortNumber = 30
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "31",
                Name = "电机编码器故障2",
                Value = 31,
                SortNumber = 31
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "32",
                Name = "电机编码器故障3",
                Value = 32,
                SortNumber = 32
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "33",
                Name = "绝对值编码器故障2",
                Value = 33,
                SortNumber = 33
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "34",
                Name = "绝对值编码器复位2",
                Value = 34,
                SortNumber = 34
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "35",
                Name = "绝对值编码器运行时复位",
                Value = 35,
                SortNumber = 35
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "36",
                Name = "行程设计距离过短",
                Value = 36,
                SortNumber = 36
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "38",
                Name = "电磁制动器故障2",
                Value = 38,
                SortNumber = 38
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "39",
                Name = "电机编码器故障4",
                Value = 39,
                SortNumber = 39
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "40",
                Name = "电机编码器故障5",
                Value = 40,
                SortNumber = 40
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "41",
                Name = "绝对值编码器位置不稳定",
                Value = 41,
                SortNumber = 41
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "42",
                Name = "设置行程时电机转向错误",
                Value = 42,
                SortNumber = 42
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "43",
                Name = "接近开关与开门位置距离过近",
                Value = 43,
                SortNumber = 43
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "44",
                Name = "行程设定距离过长",
                Value = 44,
                SortNumber = 44
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "45",
                Name = "绝对值编码器方向设定失败",
                Value = 45,
                SortNumber = 45
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "46",
                Name = "未进行出厂设置",
                Value = 46,
                SortNumber = 46
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "47",
                Name = "行程设定绝对值编码器数值不匹配",
                Value = 47,
                SortNumber = 47
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "48",
                Name = "门体位置异常",
                Value = 48,
                SortNumber = 48
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "49",
                Name = "电子行程异常",
                Value = 49,
                SortNumber = 49
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "50",
                Name = "电机过热",
                Value = 50,
                SortNumber = 50
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "51",
                Name = "启动器过热",
                Value = 51,
                SortNumber = 51
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "52",
                Name = "控制器关机",
                Value = 52,
                SortNumber = 52
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "DoorMalfunction").Id,
                Code = "53",
                Name = "电磁制动故障",
                Value = 53,
                SortNumber = 53
            });

            dictDtos.Add(new DictDto()
            {
                Code = "CarStartStop",
                Name = "车辆启停"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarStartStop").Id,
                Code = "Start",
                Name = "启动",
                Value = 1,
                SortNumber = 1
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarStartStop").Id,
                Code = "Stop",
                Name = "停止",
                Value = 2,
                SortNumber = 2
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarStartStop").Id,
                Code = "ControlStart",
                Name = "管控启动",
                Value = 11,
                SortNumber = 3
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarStartStop").Id,
                Code = "ControlStop",
                Name = "管控停止",
                Value = 12,
                SortNumber = 4
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarStartStop").Id,
                Code = "PrecisionStop",
                Name = "精准停止",
                Value = 22,
                SortNumber = 5
            });

            dictDtos.Add(new DictDto()
            {
                Code = "CarDirection",
                Name = "车辆方向"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarDirection").Id,
                Code = "1",
                Name = "原地右旋 90°完成",
                Value = 1,
                Param = "90",
                SortNumber = 1
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarDirection").Id,
                Code = "2",
                Name = "原地右旋 180°完成",
                Value = 2,
                Param = "180",
                SortNumber = 2
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarDirection").Id,
                Code = "3",
                Name = "原地右旋 270°完成",
                Value = 3,
                Param = "270",
                SortNumber = 3
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarDirection").Id,
                Code = "4",
                Name = "原地左旋 90°完成",
                Value = 4,
                Param = "-90",
                SortNumber = 4
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarDirection").Id,
                Code = "5",
                Name = "原地左旋 180°完成",
                Value = 5,
                Param = "-180",
                SortNumber = 5
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarDirection").Id,
                Code = "6",
                Name = "原地左旋 270°完成",
                Value = 6,
                Param = "-270",
                SortNumber = 6
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarDirection").Id,
                Code = "11",
                Name = "原地右旋 90°中",
                Value = 11,
                Param = "0",
                SortNumber = 7
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarDirection").Id,
                Code = "12",
                Name = "原地右旋 180°中",
                Value = 12,
                Param = "0",
                SortNumber = 8
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarDirection").Id,
                Code = "13",
                Name = "原地右旋 270°中",
                Value = 13,
                Param = "0",
                SortNumber = 9
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarDirection").Id,
                Code = "14",
                Name = "原地左旋 90°中",
                Value = 14,
                Param = "0",
                SortNumber = 10
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarDirection").Id,
                Code = "15",
                Name = "原地左旋 180°中",
                Value = 15,
                Param = "0",
                SortNumber = 11
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarDirection").Id,
                Code = "16",
                Name = "原地左旋 270°中",
                Value = 16,
                Param = "0",
                SortNumber = 12
            });

            dictDtos.Add(new DictDto()
            {
                Code = "CarOrientation",
                Name = "车辆运动"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarOrientation").Id,
                Code = "1",
                Name = "前进中",
                Value = 1,
                SortNumber = 1
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarOrientation").Id,
                Code = "2",
                Name = "后退中",
                Value = 2,
                SortNumber = 2
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarOrientation").Id,
                Code = "11",
                Name = "左横移中",
                Value = 11,
                SortNumber = 3
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarOrientation").Id,
                Code = "12",
                Name = "右横移中",
                Value = 12,
                SortNumber = 4
            });

            dictDtos.Add(new DictDto()
            {
                Code = "CarByroad",
                Name = "车辆岔道"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarByroad").Id,
                Code = "1",
                Name = "直行方向",
                Value = 1,
                SortNumber = 1
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarByroad").Id,
                Code = "2",
                Name = "右岔方向",
                Value = 2,
                SortNumber = 2
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarByroad").Id,
                Code = "3",
                Name = "左岔方向",
                Value = 3,
                SortNumber = 3
            });

            dictDtos.Add(new DictDto()
            {
                Code = "CarObstacle",
                Name = "车辆避障"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarObstacle").Id,
                Code = "1",
                Name = "关闭避障",
                Value = 1,
                SortNumber = 1
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarObstacle").Id,
                Code = "2",
                Name = "空载直行小区域",
                Value = 2,
                SortNumber = 2
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarObstacle").Id,
                Code = "3",
                Name = "空载直行大区域",
                Value = 3,
                SortNumber = 3
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarObstacle").Id,
                Code = "4",
                Name = "空载左转弯区域",
                Value = 4,
                SortNumber = 4
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarObstacle").Id,
                Code = "5",
                Name = "空载右转弯区域",
                Value = 5,
                SortNumber = 5
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarObstacle").Id,
                Code = "6",
                Name = "满载直行小区域",
                Value = 6,
                SortNumber = 6
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarObstacle").Id,
                Code = "7",
                Name = "满载直行大区域",
                Value = 7,
                SortNumber = 7
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarObstacle").Id,
                Code = "8",
                Name = "满载左转弯区域",
                Value = 8,
                SortNumber = 8
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarObstacle").Id,
                Code = "9",
                Name = "满载右转弯区域",
                Value = 9,
                SortNumber = 9
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarObstacle").Id,
                Code = "10",
                Name = "狭窄区域",
                Value = 10,
                SortNumber = 10
            });

            dictDtos.Add(new DictDto()
            {
                Code = "CarAudio",
                Name = "车辆音乐"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarAudio").Id,
                Code = "1",
                Name = "关闭音乐",
                Value = 1,
                SortNumber = 1
            });

            dictDtos.Add(new DictDto()
            {
                Code = "CarLight",
                Name = "车辆灯带"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarLight").Id,
                Code = "1",
                Name = "关闭灯带",
                Value = 1,
                SortNumber = 1
            });

            dictDtos.Add(new DictDto()
            {
                Code = "CarCharge",
                Name = "车辆充电"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarCharge").Id,
                Code = "1",
                Name = "停止充电",
                Value = 1,
                SortNumber = 1
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarCharge").Id,
                Code = "2",
                Name = "开始充电",
                Value = 2,
                SortNumber = 2
            });

            dictDtos.Add(new DictDto()
            {
                Code = "CarRest",
                Name = "车辆休眠"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarRest").Id,
                Code = "1",
                Name = "停止睡眠",
                Value = 1,
                SortNumber = 1
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarRest").Id,
                Code = "2",
                Name = "开始睡眠",
                Value = 2,
                SortNumber = 2
            });

            dictDtos.Add(new DictDto()
            {
                Code = "CarLift",
                Name = "车辆举升"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarLift").Id,
                Code = "1",
                Name = "上升到位",
                Value = 1,
                SortNumber = 1
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarLift").Id,
                Code = "2",
                Name = "下降到位",
                Value = 2,
                SortNumber = 2
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarLift").Id,
                Code = "3",
                Name = "上升中",
                Value = 3,
                SortNumber = 3
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarLift").Id,
                Code = "4",
                Name = "下降中",
                Value = 4,
                SortNumber = 4
            });

            dictDtos.Add(new DictDto()
            {
                Code = "CarClamp",
                Name = "车辆夹紧"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarClamp").Id,
                Code = "1",
                Name = "夹紧到位",
                Value = 1,
                SortNumber = 1
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarClamp").Id,
                Code = "2",
                Name = "松开到位",
                Value = 2,
                SortNumber = 2
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarClamp").Id,
                Code = "11",
                Name = "夹紧中",
                Value = 11,
                SortNumber = 3
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarClamp").Id,
                Code = "12",
                Name = "松开中",
                Value = 12,
                SortNumber = 4
            });
            dictDtos.Add(new DictDto()
            {
                Code = "Signal",
                Name = "安全信号"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "Signal").Id,
                Code = "AGV_Beat",
                Name = "AGV心跳",
                Value = 0,
                SortNumber = 1
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "Signal").Id,
                Code = "PLC_Beat",
                Name = "PLC心跳",
                Value = 0,
                SortNumber = 2
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "Signal").Id,
                Code = "AGV_AGVAlarm",
                Name = "AGV故障",
                Value = 0,
                SortNumber = 2.3
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "Signal").Id,
                Code = "AGV_Request",
                Name = "AGV请求进站",
                Value = 0,
                SortNumber = 3
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "Signal").Id,
                Code = "AGV_RequestLeave",
                Name = "AGV请求离开",
                Value = 0,
                SortNumber = 4
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "Signal").Id,
                Code = "AGV_SafeLeave",
                Name = "AGV安全离开",
                Value = 0,
                SortNumber = 5
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "Signal").Id,
                Code = "PLC_PlcAlarm",
                Name = "PLC故障",
                Value = 0,
                SortNumber = 6
            });


            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "Signal").Id,
                Code = "PLC_AllowRequest",
                Name = "PLC允许进站",
                Value = 0,
                SortNumber = 7
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "Signal").Id,
                Code = "PLC_AllowLeave",
                Name = "PLC允许离开",
                Value = 0,
                SortNumber = 8
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "Signal").Id,
                Code = "PLC_Entering",
                Name = "PLC正在进入中",
                Value = 0,
                SortNumber = 9
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "Signal").Id,
                Code = "AGV_Inplace",
                Name = "AGV到位",
                Value = 0,
                SortNumber = 10
            });

            dictDtos.Add(new DictDto()
            {
                Code = "CarTray",
                Name = "车辆托盘"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarTray").Id,
                Code = "0",
                Name = "原点",
                Value = 0,
                SortNumber = 1
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarTray").Id,
                Code = "1",
                Name = "托盘左转到位",
                Value = 1,
                SortNumber = 2
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarTray").Id,
                Code = "2",
                Name = "托盘右转到位",
                Value = 2,
                SortNumber = 3
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarTray").Id,
                Code = "11",
                Name = "托盘左转中",
                Value = 11,
                SortNumber = 4
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarTray").Id,
                Code = "12",
                Name = "托盘右转中",
                Value = 12,
                SortNumber = 5
            });

            dictDtos.Add(new DictDto()
            {
                Code = "CarRoll",
                Name = "车辆滚筒"
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarRoll").Id,
                Code = "0",
                Name = "停止中",
                Value = 0,
                SortNumber = 1
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarRoll").Id,
                Code = "1",
                Name = "接料到位",
                Value = 1,
                SortNumber = 2
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarRoll").Id,
                Code = "2",
                Name = "送料到位",
                Value = 2,
                SortNumber = 3
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarRoll").Id,
                Code = "11",
                Name = "接料中",
                Value = 11,
                SortNumber = 4
            });
            dictItemDtos.Add(new DictItemDto()
            {
                DictId = dictDtos.First(e => e.Code == "CarRoll").Id,
                Code = "12",
                Name = "送料中",
                Value = 12,
                SortNumber = 5
            });

            var dictEntities = mapper.Map<List<DictEntity>>(dictDtos);
            context.AddRange(dictEntities);
            var dictItemEntities = mapper.Map<List<DictItemEntity>>(dictItemDtos);
            context.AddRange(dictItemEntities);

            var carTypeEntities = context.Set<CarTypeEntity>().ToList();
            var carActionDtos = new List<CarActionDto>();

            var carTypeCarId = carTypeEntities.First(e => e.Code == "Car").Id;
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarId, Code = "Start", Name = "启动", SortNumber = 101 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarId, Code = "Stop", Name = "停止", SortNumber = 102 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarId, Code = "EmergencyStop", Name = "急停", SortNumber = 103 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarId, Code = "Reset", Name = "恢复", SortNumber = 104 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarId, Code = "StartStop", Name = "启停", SortNumber = 105 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarId, Code = "Direction", Name = "方向", SortNumber = 106 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarId, Code = "Orientation", Name = "运动", SortNumber = 107 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarId, Code = "Byroad", Name = "岔道", SortNumber = 108 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarId, Code = "Speed", Name = "速度", SortNumber = 109 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarId, Code = "Obstacle", Name = "避障", SortNumber = 110 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarId, Code = "Audio", Name = "音乐", SortNumber = 111 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarId, Code = "Light", Name = "灯带", SortNumber = 112 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarId, Code = "Charge", Name = "充电", SortNumber = 113 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarId, Code = "Rest", Name = "休眠", SortNumber = 114 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarId, Code = "Lift", Name = "举升", SortNumber = 115 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarId, Code = "Clamp", Name = "夹紧", SortNumber = 116 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarId, Code = "Tray", Name = "托盘", SortNumber = 117 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarId, Code = "Roll", Name = "滚筒", SortNumber = 118 });

            var carTypeCarForkliftId = carTypeEntities.First(e => e.Code == "CarForklift").Id;
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarForkliftId, Code = "Start", Name = "启动", SortNumber = 101 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarForkliftId, Code = "Stop", Name = "停止", SortNumber = 102 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarForkliftId, Code = "EmergencyStop", Name = "急停", SortNumber = 103 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarForkliftId, Code = "Reset", Name = "恢复", SortNumber = 104 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarForkliftId, Code = "StartStop", Name = "启停", SortNumber = 105 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarForkliftId, Code = "Direction", Name = "方向", SortNumber = 106 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarForkliftId, Code = "Orientation", Name = "运动", SortNumber = 107 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarForkliftId, Code = "Byroad", Name = "岔道", SortNumber = 108 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarForkliftId, Code = "Speed", Name = "速度", SortNumber = 109 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarForkliftId, Code = "Obstacle", Name = "避障", SortNumber = 110 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarForkliftId, Code = "Audio", Name = "音乐", SortNumber = 111 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarForkliftId, Code = "Light", Name = "灯带", SortNumber = 112 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarForkliftId, Code = "Charge", Name = "充电", SortNumber = 113 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarForkliftId, Code = "Rest", Name = "休眠", SortNumber = 114 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarForkliftId, Code = "Lift", Name = "举升", SortNumber = 115 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarForkliftId, Code = "Clamp", Name = "夹紧", SortNumber = 116 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarForkliftId, Code = "Tray", Name = "托盘", SortNumber = 117 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarForkliftId, Code = "Roll", Name = "滚筒", SortNumber = 118 });

            var carTypeCarConveyorId = carTypeEntities.First(e => e.Code == "CarConveyor").Id;
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarConveyorId, Code = "Start", Name = "启动", SortNumber = 101 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarConveyorId, Code = "Stop", Name = "停止", SortNumber = 102 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarConveyorId, Code = "EmergencyStop", Name = "急停", SortNumber = 103 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarConveyorId, Code = "Reset", Name = "恢复", SortNumber = 104 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarConveyorId, Code = "StartStop", Name = "启停", SortNumber = 105 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarConveyorId, Code = "Direction", Name = "方向", SortNumber = 106 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarConveyorId, Code = "Orientation", Name = "运动", SortNumber = 107 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarConveyorId, Code = "Byroad", Name = "岔道", SortNumber = 108 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarConveyorId, Code = "Speed", Name = "速度", SortNumber = 109 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarConveyorId, Code = "Obstacle", Name = "避障", SortNumber = 110 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarConveyorId, Code = "Audio", Name = "音乐", SortNumber = 111 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarConveyorId, Code = "Light", Name = "灯带", SortNumber = 112 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarConveyorId, Code = "Charge", Name = "充电", SortNumber = 113 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarForkliftId, Code = "Rest", Name = "休眠", SortNumber = 114 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarConveyorId, Code = "Lift", Name = "举升", SortNumber = 115 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarConveyorId, Code = "Clamp", Name = "夹紧", SortNumber = 116 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarConveyorId, Code = "Tray", Name = "托盘", SortNumber = 117 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarConveyorId, Code = "Roll", Name = "滚筒", SortNumber = 118 });

            var carTypeCarTuggerId = carTypeEntities.First(e => e.Code == "CarTugger").Id;
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarTuggerId, Code = "Start", Name = "启动", SortNumber = 101 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarTuggerId, Code = "Stop", Name = "停止", SortNumber = 102 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarTuggerId, Code = "EmergencyStop", Name = "急停", SortNumber = 103 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarTuggerId, Code = "Reset", Name = "恢复", SortNumber = 104 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarTuggerId, Code = "StartStop", Name = "启停", SortNumber = 105 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarTuggerId, Code = "Direction", Name = "方向", SortNumber = 106 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarTuggerId, Code = "Orientation", Name = "运动", SortNumber = 107 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarTuggerId, Code = "Byroad", Name = "岔道", SortNumber = 108 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarTuggerId, Code = "Speed", Name = "速度", SortNumber = 109 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarTuggerId, Code = "Obstacle", Name = "避障", SortNumber = 110 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarTuggerId, Code = "Audio", Name = "音乐", SortNumber = 111 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarTuggerId, Code = "Light", Name = "灯带", SortNumber = 112 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarTuggerId, Code = "Charge", Name = "充电", SortNumber = 113 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarTuggerId, Code = "Rest", Name = "休眠", SortNumber = 114 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarTuggerId, Code = "Lift", Name = "举升", SortNumber = 115 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarTuggerId, Code = "Clamp", Name = "夹紧", SortNumber = 116 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarTuggerId, Code = "Tray", Name = "托盘", SortNumber = 117 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarTuggerId, Code = "Roll", Name = "滚筒", SortNumber = 118 });

            var carTypeCarCarrierId = carTypeEntities.First(e => e.Code == "CarCarrier").Id;
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarCarrierId, Code = "Start", Name = "启动", SortNumber = 101 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarCarrierId, Code = "Stop", Name = "停止", SortNumber = 102 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarCarrierId, Code = "EmergencyStop", Name = "急停", SortNumber = 103 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarCarrierId, Code = "Reset", Name = "恢复", SortNumber = 104 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarCarrierId, Code = "StartStop", Name = "启停", SortNumber = 105 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarCarrierId, Code = "Direction", Name = "方向", SortNumber = 106 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarCarrierId, Code = "Orientation", Name = "运动", SortNumber = 107 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarCarrierId, Code = "Byroad", Name = "岔道", SortNumber = 108 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarCarrierId, Code = "Speed", Name = "速度", SortNumber = 109 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarCarrierId, Code = "Obstacle", Name = "避障", SortNumber = 110 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarCarrierId, Code = "Audio", Name = "音乐", SortNumber = 111 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarCarrierId, Code = "Light", Name = "灯带", SortNumber = 112 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarCarrierId, Code = "Charge", Name = "充电", SortNumber = 113 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarCarrierId, Code = "Rest", Name = "休眠", SortNumber = 114 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarCarrierId, Code = "Lift", Name = "举升", SortNumber = 115 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarCarrierId, Code = "Clamp", Name = "夹紧", SortNumber = 116 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarCarrierId, Code = "Tray", Name = "托盘", SortNumber = 117 });
            carActionDtos.Add(new CarActionDto() { CarTypeId = carTypeCarCarrierId, Code = "Roll", Name = "滚筒", SortNumber = 118 });

            var carActionEntities = mapper.Map<List<CarActionEntity>>(carActionDtos);
            context.AddRange(carActionEntities);

            //扩展任务模板类型(TaskTemplateType)
            var dictDtoNews = context.Set<DictEntity>().AsEnumerable().ToList();
            //var dictItemAddDtos = new List<DictItemDto>();
            //dictItemAddDtos.Add(new DictItemDto()
            //{
            //    DictId = dictDtoNews.First(e => e.Code == "TaskTemplateType").Id,
            //    Code = "LX",
            //    Name = "离心区模板",
            //    SortNumber = 2
            //});
            //dictItemAddDtos.Add(new DictItemDto()
            //{
            //    DictId = dictDtoNews.First(e => e.Code == "TaskTemplateType").Id,
            //    Code = "HG/DS",
            //    Name = "烘干/煅烧区模板",
            //    SortNumber = 3
            //});
            //var dictItemAddEntities = mapper.Map<List<DictItemEntity>>(dictItemAddDtos);
            //context.AddRange(dictItemAddEntities);

            List<TaskTemplateMdcsDto> taskTemplateMdcsDtos = new List<TaskTemplateMdcsDto>();
            taskTemplateMdcsDtos.Add(new TaskTemplateMdcsDto
            {
                Id = "Double",
                CarTypeId = context.Set<CarTypeEntity>().First(e => e.Code == "Car").Id,
                Code = "Double",
                Name = "双点",
                Type = context.Set<DictItemEntity>().AsEnumerable().ToList().OrderBy((DictItemEntity e) => e.SortNumber).First((DictItemEntity e) => e.DictId == dictDtoNews.First((DictEntity e) => e.Code == "TaskTemplateType").Id).Code,
                Priority = 0.0
            });
            var taskTemplateMdcsEntities = mapper.Map<List<TaskTemplateMdcsEntity>>(taskTemplateMdcsDtos);
            context.AddRange(taskTemplateMdcsEntities);

            context.SaveChanges();

            return context;
        }
    }
}
