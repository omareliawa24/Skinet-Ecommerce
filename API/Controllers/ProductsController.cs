using System;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")] // becomes: api/products
public class ProductsController(IProductRepository repository) : ControllerBase
{


    [HttpGet]   // GET: api/products
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
    {
        return Ok(await repository.GetProductsAsync(brand,type,sort));
    }

    [HttpGet("{id:int}")]   // GET: api/products/2
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await repository.GetProductsByIdAsync(id);
        if (product == null) return NotFound();
        return product;
    }

    [HttpPost]   // POST: api/products
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        repository.AddProduct(product);
        if (await repository.SaveChangesAsync())
        {
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        return BadRequest("Problem Creating product");
    }

    [HttpPut("{id:int}")]   // api/products/2
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (product.Id != id || !repository.ProductExists(id))
            return BadRequest("Can't update this product");

        repository.UpdateProduct(product);
        if (await repository.SaveChangesAsync())
        {
            return NoContent();

        }
        return BadRequest("Problem Update the product");
    }
    private bool ProductExists(int id)
    {
        return repository.ProductExists(id);
    }

    [HttpDelete("{id:int}")]   // api/products/2
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await repository.GetProductsByIdAsync(id);
        if (product == null) return NotFound();

        repository.DeleteProduct(product);
        if (await repository.SaveChangesAsync())
        {
            return NoContent();

        }
        return BadRequest("Problem Deleting the product");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        return Ok(await repository.GetBrandsAsync());
    }

    [HttpGet("Types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        return Ok(await repository.GetTypesAsync());
    }

}
