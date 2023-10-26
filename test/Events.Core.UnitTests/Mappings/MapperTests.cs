using AutoMapper;
using Events.Core.Mappings;

namespace Events.Core.UnitTests.Mappings
{
    public class MapperTests
    {
        private readonly IMapper _mapper;

        public MapperTests()
        {
            _mapper = MapperSetup.ConfigureAndCreateMapper();
        }

        [Fact]
        public void MapEvent()
        {

        }
    }
}
