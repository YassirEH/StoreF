using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using webApi.Application.Services;

namespace webApi.Controllers
{
    public class APIController : ControllerBase
    {
        protected readonly IMapper _mapper;
        protected readonly INotificationService _notificationService;

        public APIController(IMapper mapper, INotificationService notificationService)
        {
            _mapper = mapper;
            _notificationService = notificationService;
        }


        protected new IActionResult Response(object? obj)
        {
            return obj == null ? NoContent() : Ok(obj);
        }
    }
}
