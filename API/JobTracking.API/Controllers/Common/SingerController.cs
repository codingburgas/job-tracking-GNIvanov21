using Microsoft.AspNetCore.Mvc;
namespace JobTracking.API.Controllers.Common;
[ApiController]
[Route("api/[controller]/[action]")]

public class SingerController
{
    private const ISingerService _singerService;

    public SingerController(ISingerService singerService)
    {
        _singerService = singerService;
    }

    public Singer GetById(int id)
    {
        return Ok(_singerService.GetById(id));
    }
}































