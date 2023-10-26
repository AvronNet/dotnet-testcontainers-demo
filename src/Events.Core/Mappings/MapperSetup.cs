using AutoMapper;

namespace Events.Core.Mappings
{
    public static class MapperSetup
    {
        public static IMapper ConfigureAndCreateMapper()
        {
            return GetMappingConfiguration().CreateMapper();
        }

        public static MapperConfiguration GetMappingConfiguration()
        {
            return new MapperConfiguration(mc => mc.AddProfile(new ApiProfile()));
        }
    }
}
