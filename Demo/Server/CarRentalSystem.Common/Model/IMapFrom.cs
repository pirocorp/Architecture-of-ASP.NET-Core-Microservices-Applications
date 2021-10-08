namespace CarRentalSystem.Common.Model
{
    using AutoMapper;

    public interface IMapFrom<T>
    {
        public void Mapping(Profile mapper) => mapper.CreateMap(typeof(T), this.GetType());
    }
}
