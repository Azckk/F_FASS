using AutoMapper;
using FASS.Data.Dtos.Warehouse;
using FASS.Data.Entities.Warehouse;
using FASS.Data.Models.Warehouse;
using FASS.Service.Dtos.Warehouse;
using FASS.Service.Entities.Warehouse;
using FASS.Service.Models.Warehouse;

namespace FASS.Data.Mappers
{
    public class WarehouseMapperProfile : Profile
    {
        public WarehouseMapperProfile()
        {
            AllowNullCollections = true;
            AllowNullDestinationValues = true;

            CreateMap<AreaEntity, AreaDto>().ReverseMap();

            CreateMap<StorageEntity, StorageDto>()
                .ForMember(dest => dest.AreaCode, opt => opt.MapFrom(src => src.Area.Code));
            CreateMap<StorageDto, StorageEntity>();
            CreateMap<ContainerEntity, ContainerDto>()
                .ForMember(dest => dest.AreaCode, opt => opt.MapFrom(src => src.Area.Code));
            CreateMap<ContainerDto, ContainerEntity>();
            CreateMap<MaterialEntity, MaterialDto>().ReverseMap();

            CreateMap<StorageContainerEntity, StorageContainerDto>().ReverseMap();
            CreateMap<ContainerMaterialEntity, ContainerMaterialDto>().ReverseMap();
            CreateMap<MaterialStorageEntity, MaterialStorageDto>().ReverseMap();

            CreateMap<StorageContainerHistoryEntity, StorageContainerHistoryDto>()
                .ForMember(dest => dest.StorageCode, opt => opt.MapFrom(src => src.Storage.Code))
                .ForMember(dest => dest.ContainerCode, opt => opt.MapFrom(src => src.Container.Code));
            CreateMap<StorageContainerHistoryDto, StorageContainerHistoryEntity>();
            CreateMap<ContainerMaterialHistoryEntity, ContainerMaterialHistoryDto>()
                .ForMember(dest => dest.ContainerCode, opt => opt.MapFrom(src => src.Container.Code))
                .ForMember(dest => dest.MaterialCode, opt => opt.MapFrom(src => src.Material.Code));
            CreateMap<ContainerMaterialHistoryDto, ContainerMaterialHistoryEntity>();
            CreateMap<MaterialStorageHistoryEntity, MaterialStorageHistoryDto>()
                .ForMember(dest => dest.MaterialCode, opt => opt.MapFrom(src => src.Material.Code))
                .ForMember(dest => dest.StorageCode, opt => opt.MapFrom(src => src.Storage.Code));
            CreateMap<MaterialStorageHistoryDto, MaterialStorageHistoryEntity>();

            CreateMap<WorkEntity, WorkDto>()
                .ForMember(dest => dest.ContainerCode, opt => opt.MapFrom(src => src.Container.Code));
            CreateMap<WorkDto, WorkEntity>();

            CreateMap<TagEntity, TagDto>().ReverseMap();

            CreateMap<StorageTagEntity, StorageTagDto>()
                .ForMember(dest => dest.StorageCode, opt => opt.MapFrom(src => src.Storage.Code))
                .ForMember(dest => dest.TagCode, opt => opt.MapFrom(src => src.Tag.Name));
            CreateMap<StorageTagDto, StorageTagEntity>();

            CreateMap<PreMaterialEntity, PreMaterialDto>().ReverseMap();
            CreateMap<PreWorkEntity, PreWorkDto>().ReverseMap();

            CreateMap<AreaEntity, Area>();
            CreateMap<Area, AreaEntity>()
                .ForMember(dest => dest.Storages, opt => opt.Ignore())
                .ForMember(dest => dest.Containers, opt => opt.Ignore());

            CreateMap<StorageEntity, Storage>();
            CreateMap<Storage, StorageEntity>()
                .ForMember(dest => dest.Area, opt => opt.Ignore())
                .ForMember(dest => dest.StorageContainers, opt => opt.Ignore())
                .ForMember(dest => dest.MaterialStorages, opt => opt.Ignore())
                .ForMember(dest => dest.StorageContainerHistorys, opt => opt.Ignore())
                .ForMember(dest => dest.MaterialStorageHistorys, opt => opt.Ignore());
            CreateMap<ContainerEntity, Container>();
            CreateMap<Container, ContainerEntity>()
                .ForMember(dest => dest.Area, opt => opt.Ignore())
                .ForMember(dest => dest.ContainerMaterials, opt => opt.Ignore())
                .ForMember(dest => dest.StorageContainers, opt => opt.Ignore())
                .ForMember(dest => dest.ContainerMaterialHistorys, opt => opt.Ignore())
                .ForMember(dest => dest.StorageContainerHistorys, opt => opt.Ignore())
                .ForMember(dest => dest.Works, opt => opt.Ignore());
            CreateMap<MaterialEntity, Material>();
            CreateMap<Material, MaterialEntity>()
                .ForMember(dest => dest.MaterialStorages, opt => opt.Ignore())
                .ForMember(dest => dest.ContainerMaterials, opt => opt.Ignore())
                .ForMember(dest => dest.MaterialStorageHistorys, opt => opt.Ignore())
                .ForMember(dest => dest.ContainerMaterialHistorys, opt => opt.Ignore());

            CreateMap<StorageContainerEntity, StorageContainer>();
            CreateMap<StorageContainer, StorageContainerEntity>()
                .ForMember(dest => dest.Storage, opt => opt.Ignore())
                .ForMember(dest => dest.Container, opt => opt.Ignore());
            CreateMap<ContainerMaterialEntity, ContainerMaterial>();
            CreateMap<ContainerMaterial, ContainerMaterialEntity>()
                .ForMember(dest => dest.Container, opt => opt.Ignore())
                .ForMember(dest => dest.Material, opt => opt.Ignore());
            CreateMap<MaterialStorageEntity, MaterialStorage>();
            CreateMap<MaterialStorage, MaterialStorageEntity>()
                .ForMember(dest => dest.Material, opt => opt.Ignore())
                .ForMember(dest => dest.Storage, opt => opt.Ignore());

            CreateMap<StorageContainerHistoryEntity, StorageContainerHistory>();
            CreateMap<StorageContainerHistory, StorageContainerHistoryEntity>()
                .ForMember(dest => dest.Storage, opt => opt.Ignore())
                .ForMember(dest => dest.Container, opt => opt.Ignore());
            CreateMap<ContainerMaterialHistoryEntity, ContainerMaterialHistory>();
            CreateMap<ContainerMaterialHistory, ContainerMaterialHistoryEntity>()
                .ForMember(dest => dest.Container, opt => opt.Ignore())
                .ForMember(dest => dest.Material, opt => opt.Ignore());
            CreateMap<MaterialStorageHistoryEntity, MaterialStorageHistory>();
            CreateMap<MaterialStorageHistory, MaterialStorageHistoryEntity>()
                .ForMember(dest => dest.Material, opt => opt.Ignore())
                .ForMember(dest => dest.Storage, opt => opt.Ignore());

            CreateMap<WorkEntity, Work>();
            CreateMap<Work, WorkEntity>()
                .ForMember(dest => dest.Container, opt => opt.Ignore());

            CreateMap<TagEntity, Tag>();

            CreateMap<StorageTagEntity, StorageTag>();
            CreateMap<StorageTag, StorageTagEntity>()
                .ForMember(dest => dest.Storage, opt => opt.Ignore())
                .ForMember(dest => dest.Tag, opt => opt.Ignore());

            CreateMap<PreMaterialEntity, PreMaterial>();
            CreateMap<PreWorkEntity, PreWork>();
        }
    }
}