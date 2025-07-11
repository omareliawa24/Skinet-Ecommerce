using System;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class ProductSpecification : BaseSpecifications<Product>
{


    public ProductSpecification(ProductSpecParams productSpecParams)
        : base(x =>
        (productSpecParams.Brands.Count == 0 || productSpecParams.Brands.Contains(x.Brand)) &&
        (productSpecParams.Types.Count == 0 || productSpecParams.Types.Contains(x.Type))&&
        (string.IsNullOrEmpty(productSpecParams.Search)||x.Name.ToLower().Contains(productSpecParams.Search))
        )
    {
        ApplyPaging(productSpecParams.PageSize * (productSpecParams.PageIndex - 1), productSpecParams.PageSize);
        switch (productSpecParams.Sort)
        {
            case "priceAsc":
                AddOrderBy(x => x.Price);
                break;
            case "priceDesc":
                AddOrderByDescending(x => x.Price);
                break;
            default:
                AddOrderBy(x => x.Name);
                break;
        }
    }


}
