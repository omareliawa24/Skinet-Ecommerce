using System;
using API.RequestHelper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")] // becomes: api/products
public class ProductsController(IGenericRepository<Product> repository) : BaseApiController
{


    [HttpGet]   // GET: api/products
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts([FromQuery]ProductSpecParams productSpecParams)
    {
        var spec = new ProductSpecification(productSpecParams);

        return await CreatePagedResult(repository,spec,productSpecParams.PageIndex,productSpecParams.PageSize);
    }

    [HttpGet("{id:int}")]   // GET: api/products/2
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await repository.GetByIdAsync(id);
        if (product == null) return NotFound();
        return product;
    }

    [HttpPost]   // POST: api/products
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        repository.Add(product);
        if (await repository.SaveAllAsync())
        {
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        return BadRequest("Problem Creating product");
    }

    [HttpPut("{id:int}")]   // api/products/2
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (product.Id != id || !repository.Exists(id))
            return BadRequest("Can't update this product");

        repository.Update(product);
        if (await repository.SaveAllAsync())
        {
            return NoContent();

        }
        return BadRequest("Problem Update the product");
    }
    private bool ProductExists(int id)
    {
        return repository.Exists(id);
    }

    [HttpDelete("{id:int}")]   // api/products/2
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await repository.GetByIdAsync(id);
        if (product == null) return NotFound();

        repository.Remove(product);
        if (await repository.SaveAllAsync())
        {
            return NoContent();

        }
        return BadRequest("Problem Deleting the product");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        var spec = new BrandListSpecification();

        return Ok(await repository.ListAsync(spec));
    }

    [HttpGet("Types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        var spec = new TypeListSpecification();

        return Ok(await repository.ListAsync(spec));
    }

}
