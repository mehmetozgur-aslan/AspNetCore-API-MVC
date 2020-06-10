using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NLayerProject.Core.Models;
using NLayerProject.Core.Services;
using NLayerProject.WebUI.APIServices;
using NLayerProject.WebUI.DTOs;
using NLayerProject.WebUI.Filters;

namespace NLayerProject.WebUI.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly CategoryApiService _categoryApiService;

        public CategoryController(ICategoryService categoryService, IMapper mapper, CategoryApiService categoryApiService)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _categoryApiService = categoryApiService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryApiService.GetAllAsync();

            return View(_mapper.Map<IEnumerable<CategoryDto>>(categories));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryDto categoryDto)
        {
            await _categoryApiService.AddAsync(categoryDto);

            return RedirectToAction("Index");
        }

        //update/5
        public async Task<IActionResult> Update(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            return View(_mapper.Map<CategoryDto>(category));
        }

        [HttpPost]
        public IActionResult Update(CategoryDto categoryDto)
        {
            _categoryService.Update(_mapper.Map<Category>(categoryDto));

            return RedirectToAction("Index");
        }

        [ServiceFilter(typeof(CategoryNotFoundFilter))]
        public IActionResult Delete(int id)
        {
            var category = _categoryService.GetByIdAsync(id).Result;
            _categoryService.Remove(category);

            return RedirectToAction("Index");
        }
    }
}
