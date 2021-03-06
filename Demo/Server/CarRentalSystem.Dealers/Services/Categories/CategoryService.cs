namespace CarRentalSystem.Dealers.Services.Categories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using Common.Services.Data;
    using Common.Services.Messages;
    using Data;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Models.Categories;

    public class CategoryService : DataService<Category>, ICategoryService
    {
        private readonly IMapper mapper;

        public CategoryService(DealersDbContext db, IPublisher publisher, IMapper mapper) 
            : base(db, publisher) 
            => this.mapper = mapper;

        public async Task<Category> Find(int categoryId)
            => await this.Data.FindAsync<Category>(categoryId);

        public async Task<IEnumerable<CategoryOutputModel>> GetAll()
            => await this.mapper
                .ProjectTo<CategoryOutputModel>(this.All())
                .ToListAsync();
    }
}
