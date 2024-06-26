namespace web_services_unit_tests
{
    using AutoMapper;
    using learndotnetfast_web_services.Entities;
    using learndotnetfast_web_services.DTOs;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CourseModule, CourseModuleDTO>().ReverseMap();
            CreateMap<Tutorial, TutorialDTO>().ReverseMap();
            CreateMap<Progress, ProgressDTO>().ReverseMap();
            CreateMap<Progress, TutorialCompletionDTO>().ReverseMap();
        }
    }

}
