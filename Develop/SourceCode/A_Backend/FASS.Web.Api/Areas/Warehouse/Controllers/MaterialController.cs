using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using FASS.Data.Dtos.Warehouse;
using FASS.Service.Services.Warehouse.Interfaces;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Common.NETCore;

namespace FASS.Web.Api.Areas.Warehouse.Controllers
{
    [Route("api/v1/Warehouse/[controller]/[action]")]
    [Tags("仓储管理-物料管理")]
    public class MaterialController : BaseController
    {
        private readonly ILogger<MaterialController> _logger;
        private readonly IMaterialService _materialService;
        private readonly IContainerService _containerService;
        public MaterialController(
            ILogger<MaterialController> logger,
            IMaterialService materialService,
            IContainerService containerService)
        {
            _logger = logger;
            _materialService = materialService;
            _containerService = containerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _materialService.ToPageAsync(param);
           
            var data = new
            {
                rows = page.Data,
                total = page.TotalCount
            };
            return Ok(data);
        }
        [HttpGet]
        public async Task<IActionResult> GetPageSelect([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _materialService.ToPageAsync(param);
            var fileterDate = page.Data.Where(e => !e.IsLock).ToList();
            var data = new
            {
                rows = fileterDate,
                total = page.TotalCount
            };
            return Ok(data);
        }
        [HttpGet]
        public async Task<IActionResult> GetEntity([FromQuery] string keyValue)
        {
            var data = await _materialService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] MaterialDto materialDto)
        {
            await _materialService.AddOrUpdateAsync(keyValue, materialDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            try
            {
                await _materialService.DeleteAsync(keyValues);
                return Ok();
            }
            catch (Exception)
            {

                return BadRequest("物料跟容器有绑定关系或者历史信息里面有记录");
            }
           
        }

        [HttpGet]
        public async Task<IActionResult> GetListToSelect()
        {
            var data = await _materialService.Set().Where(e => e.IsEnable && e.IsLock).OrderByDescending(e => e.CreateAt).ToListAsync();
            return Ok(data);
        }

        //[HttpGet]
        //public IActionResult Select()
        //{
        //    return View();
        //}

        [HttpGet]
        public async Task<IActionResult> SelectGetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _materialService.SelectGetPageAsync(param);
            var data = new
            {
                rows = page.Data,
                total = page.TotalCount
            };
            return Ok(data);
        }

        //[HttpGet]
        //public IActionResult Storage()
        //{
        //    return View();
        //}

        [HttpGet]
        public async Task<IActionResult> StorageGetPage([FromQuery] string keyValue, [FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _materialService.StorageGetPageAsync(keyValue, param);
            var data = new
            {
                rows = page.Data,
                total = page.TotalCount
            };
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> StorageAdd([FromQuery] string keyValue, [FromBody] List<StorageDto> storageDtos)
        {
            await _materialService.StorageAddAsync(keyValue, storageDtos);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> StorageDelete([FromQuery] string keyValue, [FromBody] List<StorageDto> storageDtos)
        {
            await _materialService.StorageDeleteAsync(keyValue, storageDtos);
            return Ok();
        }

        //[HttpGet]
        //public IActionResult Container()
        //{
        //    return View();
        //}

        [HttpPut]
        public async Task<IActionResult> MaterialAddBand([FromQuery] string keyValue, [FromBody] MaterialDto materialDto)
        {
            await _materialService.AddAsync(materialDto);
            var materialDtos = _materialService.Set().Where(m => m.Code == materialDto.Code).ToList();
            if (keyValue != null)
            {
                var result = await _containerService.MaterialAddAsync(keyValue, materialDtos);
                if (result == 0)
                {
                    return BadRequest("容器已绑定物料，请解绑后再绑定");
                }
            }
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> ContainerGetPage([FromQuery] string keyValue, [FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _materialService.ContainerGetPageAsync(keyValue, param);
            var data = new
            {
                rows = page.Data,
                total = page.TotalCount
            };
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> ContainerAdd([FromQuery] string keyValue, [FromBody] List<ContainerDto> containerDtos)
        {
            if (containerDtos.Count>1)
            {
                return BadRequest("一个物料只能加入一个容器中");
            }
           var result= await _materialService.ContainerAddAsync(keyValue, containerDtos);
            if (result==0)
            {
                return BadRequest("该容器已绑定物料或者该容器不存在");
            }
          
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> ContainerDelete([FromQuery] string keyValue, [FromBody] List<ContainerDto> containerDtos)
        {
            if (containerDtos.Count > 1) {
                return BadRequest("删除容器数量不对，一个物料只会有一个容器");
            };
            var result=await _materialService.ContainerDeleteAsync(keyValue, containerDtos);
            if (result==0)
            {
                return BadRequest("找不到需要删除的物料或者容器");
            }
            return Ok();
        }
    }
}
