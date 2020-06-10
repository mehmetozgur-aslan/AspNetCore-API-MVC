using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLayerProject.Core.Services;
using NLayerProject.WebUI.APIServices;
using NLayerProject.WebUI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NLayerProject.WebUI.Filters
{
    public class CategoryNotFoundFilter : ActionFilterAttribute
    {
        private readonly CategoryApiService _categoryService;

        public CategoryNotFoundFilter(CategoryApiService categoryService)
        {
            _categoryService = categoryService;
        }

        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            int id = (int)context.ActionArguments.Values.FirstOrDefault();

            var product = await _categoryService.GetByIdAsync(id);

            if (product != null)
            {
                await next();
            }
            else
            {
                ErrorDto errorDto = new ErrorDto();

                errorDto.Errors.Add($"id'si {id} olan kategori veritabanında bulunamadı.");

                context.Result = new RedirectToActionResult("Error","Home", errorDto);
            }
        }

    }
}
