using AutoMapper;

namespace Main.Global.Library.AutoMapper
{
    public static class MapperExtensions
    {
        private static IMapper _mapper;
        static MapperExtensions()
        {
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>())
                .CreateMapper();
        }

        public static TOut Map<TIn, TOut>(this TIn? source)
            => _mapper.Map<TOut>(source);

        public static IEnumerable<TOut> Map<TIn, TOut>(this IEnumerable<TIn>? source)
            => _mapper.Map<IEnumerable<TOut>>(source);
    }
}