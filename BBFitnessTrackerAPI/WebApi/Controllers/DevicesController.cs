using Core.Contracts;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Devices.Web.ApiControllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        IUnitOfWork _unitOfWork;
        public DevicesController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

    }
}
