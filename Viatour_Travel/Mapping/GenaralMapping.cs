using AutoMapper;
using Viatour_Travel.Dtos.CategoryDtos;
using Viatour_Travel.Dtos.ReservationDtos;
using Viatour_Travel.Dtos.ReviewDtos;
using Viatour_Travel.Dtos.TourDtos;
using Viatour_Travel.Dtos.TourImageDtos;
using Viatour_Travel.Entities;

namespace Viatour_Travel.Mapping
{
    public class GenaralMapping : Profile
    {
        public GenaralMapping()
        {
            CreateMap<Category, ResultCategoryDto>().ReverseMap();
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();
            CreateMap<Category, GetCategoryByIdDto>().ReverseMap();

            CreateMap<Tour, CreateTourDto>().ReverseMap();
            CreateMap<Tour, ResultTourDto>().ReverseMap();
            CreateMap<Tour, UpdateTourDto>().ReverseMap();
            CreateMap<Tour, GetTourByIdDto>().ReverseMap();
            CreateMap<Tour, GetTourDetailDto>().ReverseMap();

            CreateMap<Review, CreateReviewDto>().ReverseMap();
            CreateMap<Review, ResultReviewDto>().ReverseMap();
            CreateMap<Review, ResultReviewByTourIdDto>().ReverseMap();
            CreateMap<Review, GetReviewByIdDto>().ReverseMap();

            CreateMap<TourPlan, TourPlanDto>().ReverseMap();
            CreateMap<GetTourByIdDto, UpdateTourDto>().ReverseMap();
            CreateMap<TourImage, ResultTourImageDto>().ReverseMap();
            CreateMap<TourImage, CreateTourImageDto>().ReverseMap();

            CreateMap<Reservation, CreateReservationDto>().ReverseMap();
            CreateMap<Reservation, ResultReservationDto>().ReverseMap();
            CreateMap<Reservation, GetReservationByIdDto>().ReverseMap();
        }
    }
}