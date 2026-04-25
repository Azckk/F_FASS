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
    [Tags("仓储管理-容器管理")]
    public class ContainerController : BaseController
    {
        private readonly ILogger<ContainerController> _logger;
        private readonly IContainerService _containerService;

        public ContainerController(
            ILogger<ContainerController> logger,
            IContainerService containerService)
        {
            _logger = logger;
            _containerService = containerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _containerService.ToPageAsync(param);
          
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
            var page = await _containerService.ToPageAsync(param);
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
            var data = await _containerService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] ContainerDto containerDto)
        {
           var result= await _containerService.AddOrUpdateAsync(keyValue, containerDto);
            if (result==0)
            {
                return BadRequest("修改容器状态前先解除绑定的物料");
            }
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            try
            {
                await _containerService.DeleteAsync(keyValues);
                return Ok();
            }
            catch (Exception)
            {

                return BadRequest("容器跟物料、库位有绑定关系或者历史信息里面有记录");
            }
           
        }

        [HttpGet]
        public async Task<IActionResult> GetListToSelect()
        {
            //获取所有绑定的

            var data = await _containerService.Set().Where(e => e.IsEnable && e.IsLock).OrderByDescending(e => e.CreateAt).ToListAsync();
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
            var page = await _containerService.SelectGetPageAsync(param);
            var data = new
            {
                rows = page.Data,
                total = page.TotalCount
            };
            return Ok(data);
        }

        //[HttpGet]
        //public IActionResult Material()
        //{
        //    return View();
        //}

        [HttpGet]
        public async Task<IActionResult> MaterialGetPage([FromQuery] string keyValue, [FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _containerService.MaterialGetPageAsync(keyValue, param);
            var data = new
            {
                rows = page.Data,
                total = page.TotalCount
            };
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> MaterialAdd([FromQuery] string keyValue, [FromBody] List<MaterialDto> materialDtos)
        {
      
           var result= await _containerService.MaterialAddAsync(keyValue, materialDtos);
            if (result==0)
            {
                return BadRequest("容器已绑定物料，请解绑后再绑定");
            }
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> MaterialDelete([FromQuery] string keyValue, [FromBody] List<MaterialDto> materialDtos)
        {
            await _containerService.MaterialDeleteAsync(keyValue, materialDtos);
            return Ok();
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
            var page = await _containerService.StorageGetPageAsync(keyValue, param);
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
            //先查容器是否已经绑定库位
            var containerDto = Guard.NotNull(await _containerService.FirstOrDefaultAsync(keyValue));
            var selectedStorageDtos = await _containerService.GetStoragesAsync(containerDto);
            if (selectedStorageDtos.Count() > 0 || storageDtos.Count > 1) 
            {
                return BadRequest("一个容器只允许绑定一个库位");
            }
            var result= await _containerService.StorageAddAsync(keyValue, storageDtos);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> StorageDelete([FromQuery] string keyValue, [FromBody] List<StorageDto> storageDtos)
        {
            await _containerService.StorageDeleteAsync(keyValue, storageDtos);
            return Ok();
        }
    }
}
