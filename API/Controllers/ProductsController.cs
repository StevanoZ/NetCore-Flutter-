using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dto;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController : BaseApIController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPhotoRepository _photoRepository;
        public ProductsController(IUnitOfWork unitOfWork, IPhotoRepository photoRepository, IMapper mapper)
        {
            _photoRepository = photoRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams specParams)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(specParams);
            var countSpec = new ProductWithFiltersForCountSpecificication(specParams);
            var products = await _unitOfWork.Repository<Product>().ListAllWithSpecAsync(spec);
            var totalItems = await _unitOfWork.Repository<Product>().CountAsync(countSpec);
            var data = _mapper.Map<IReadOnlyList<ProductToReturnDto>>(products);
            return Ok(new Pagination<ProductToReturnDto>(specParams.PageIndex, specParams.PageSize, totalItems, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await _unitOfWork.Repository<Product>().GetByIdWithSpecAsync(spec);
            if (product == null) return NotFound(new ApiResponse(404));
            var data = _mapper.Map<ProductToReturnDto>(product);
            return Ok(data);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _unitOfWork.Repository<ProductBrand>().ListAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _unitOfWork.Repository<ProductType>().ListAllAsync());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductToReturnDto>> CreateProduct(ProductCreateDto productCreateDto)
        {
            var product = _mapper.Map<Product>(productCreateDto);
            _unitOfWork.Repository<Product>().Add(product);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem creating product"));
            return Ok(_mapper.Map<ProductToReturnDto>(product));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> UpdateProduct(int id, ProductCreateDto productUpdateDto)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
            if (product == null) return NotFound(new ApiResponse(404));
            _mapper.Map(productUpdateDto, product);
            _unitOfWork.Repository<Product>().Update(product);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, "Error updating product"));
            return Ok(_mapper.Map<ProductToReturnDto>(product));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
            if (product == null)
            {
                return NotFound(new ApiResponse(404));
            }
            foreach (var photo in product.Photos)
            {
                if (photo.Id > 18)
                {
                    _photoRepository.DeleteFromDisk(photo);
                }
            }
            _unitOfWork.Repository<Product>().Remove(product);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, "Error deleting product"));
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/photo")]
        public async Task<ActionResult<ProductToReturnDto>> AddProductPhoto(int id, [FromForm] PhotoUploadDto photoDto)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await _unitOfWork.Repository<Product>().GetByIdWithSpecAsync(spec);
            if (photoDto.Photo.Length > 0)
            {
                var photo = await _photoRepository.SaveToDiskAsync(photoDto.Photo);
                if (photo != null)
                {
                    product.AddPhoto(photo.PictureUrl, photo.FileName);
                    _unitOfWork.Repository<Product>().Update(product);
                    var result = await _unitOfWork.Complete();
                    if (result <= 0) return BadRequest(new ApiResponse(400, "Problem uploading product photo"));
                }
                else
                {
                    return BadRequest(new ApiResponse(400, "Problem uploading product photo"));
                }
            }
            return _mapper.Map<ProductToReturnDto>(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/photo/{photoId}")]
        public async Task<IActionResult> DeleteProductPhoto(int id, int photoId)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await _unitOfWork.Repository<Product>().GetByIdWithSpecAsync(spec);

            var photo = product.Photos.SingleOrDefault(p => p.Id == photoId);
            if (photo != null)
            {
                if (photo.IsMain)
                {
                    return BadRequest(new ApiResponse(400, "You cannot delete the main photo"));
                }
                _photoRepository.DeleteFromDisk(photo);
            }
            else
            {
                return NotFound(new ApiResponse(404, "Photo does not exist"));
            }
            product.RemovePhoto(photo.Id);
            _unitOfWork.Repository<Product>().Update(product);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem deleting photo"));

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{id}/photo/{photoId}")]
        public async Task<ActionResult<ProductToReturnDto>> SetMainPhoto(int id, int photoId)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await _unitOfWork.Repository<Product>().GetByIdWithSpecAsync(spec);
            if (product.Photos.All(p => p.Id != photoId)) return NotFound(new ApiResponse(404));
            product.SetMainPhoto(photoId);
            _unitOfWork.Repository<Product>().Update(product);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem setting main photo"));

            return Ok(_mapper.Map<ProductToReturnDto>(product));
        }

    }
}