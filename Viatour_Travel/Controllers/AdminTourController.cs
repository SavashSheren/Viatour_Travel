using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Viatour_Travel.Dtos.TourDtos;
using Viatour_Travel.Services.TourServices;
using Viatour_Travel.Services.UploadService;

namespace Viatour_Travel.Controllers
{
    public class AdminTourController : Controller
    {
        private readonly ITourService _tourservice;
        private readonly IFileUploadService _fileUploadService;
        private readonly IMapper _mapper;

        public AdminTourController(
            ITourService tourservice,
            IFileUploadService fileUploadService,
            IMapper mapper)
        {
            _tourservice = tourservice;
            _fileUploadService = fileUploadService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> TourList()
        {
            var values = await _tourservice.GetAllTourAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateTour()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTour(CreateTourDto createTourDto)
        {
            if (createTourDto.MapImageFile != null)
            {
                try
                {
                    var mapImageUrl = await _fileUploadService.SaveImageAsync(createTourDto.MapImageFile, "maps");
                    createTourDto.MapLocationImageUrl = mapImageUrl;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("MapImageFile", ex.Message);
                    return View(createTourDto);
                }
            }

            if (!ModelState.IsValid)
                return View(createTourDto);

            await _tourservice.CreateTourAsync(createTourDto);
            return RedirectToAction(nameof(TourList));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteTour(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return RedirectToAction(nameof(TourList));

            await _tourservice.DeleteTourAsync(id);
            return RedirectToAction(nameof(TourList));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateTour(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return RedirectToAction(nameof(TourList));

            var value = await _tourservice.GetTourByIdAsync(id);

            if (value == null)
                return RedirectToAction(nameof(TourList));

            var updateTourDto = _mapper.Map<UpdateTourDto>(value);

            return View(updateTourDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTour(UpdateTourDto updateTourDto)
        {
            if (updateTourDto.MapImageFile != null)
            {
                try
                {
                    var mapImageUrl = await _fileUploadService.SaveImageAsync(updateTourDto.MapImageFile, "maps");
                    updateTourDto.MapLocationImageUrl = mapImageUrl;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("MapImageFile", ex.Message);
                    return View(updateTourDto);
                }
            }

            if (!ModelState.IsValid)
                return View(updateTourDto);

            try
            {
                await _tourservice.UpdateTourAsync(updateTourDto);
                return RedirectToAction(nameof(TourList));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(updateTourDto);
            }
        }
    }
}