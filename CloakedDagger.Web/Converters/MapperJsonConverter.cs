using System;
using AutoMapper;
using Newtonsoft.Json;

namespace CloakedDagger.Web.Converters
{
    public class MapperJsonConverter<TEntity, TViewModel> : JsonConverter<TEntity>
    {

        protected readonly IMapper _mapper;

        public MapperJsonConverter(IMapper mapper)
        {
            this._mapper = mapper;
        }

        public override void WriteJson(JsonWriter writer, TEntity value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, _mapper.Map<TEntity, TViewModel>(value));
        }

        public override TEntity ReadJson(JsonReader reader, Type objectType, TEntity existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            return default(TEntity);
        }
    }
}