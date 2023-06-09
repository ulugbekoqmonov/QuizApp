using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public abstract class ApiControllerBase<T>:ControllerBase
    {
        protected IMapper _mapper => HttpContext.RequestServices.GetRequiredService<IMapper>();

        protected IValidator<T> _validator => HttpContext.RequestServices.GetRequiredService<IValidator<T>>();
    }
}
