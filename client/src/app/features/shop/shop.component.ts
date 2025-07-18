import { Component, inject, OnInit } from '@angular/core';
import { ShopService } from '../../core/services/shop.service';
import { ProductItemComponent } from './product-item/product-item.component';
import { MatDialog } from '@angular/material/dialog';
import { FiltersDialogComponent } from './filters-dialog/filters-dialog.component';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { PageEvent } from '@angular/material/paginator';
import { FormsModule } from '@angular/forms';
import { Product } from '../../../shared/models/product';
import { Pagination } from '../../../shared/models/Pagination';
import { ShopParams } from '../../../shared/models/shop-params';
import { MatMenuModule } from '@angular/material/menu';
import { MatSelectionList } from '@angular/material/list';
import { MatListModule } from '@angular/material/list';
import { MatPaginatorModule } from '@angular/material/paginator';

@Component({
  selector: 'app-shop',
  imports: [
    ProductItemComponent,
    MatListModule,
    MatButton,
    MatIcon,
    FormsModule,
    MatMenuModule,
    MatSelectionList,
    MatPaginatorModule
],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss',
})
export class ShopComponent implements OnInit {
  private shopService = inject(ShopService);
  private dialogService = inject(MatDialog);
  products?: Pagination<Product>;
  selectedSort: string = 'name';
  sortOptions = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'Price: Low-High', value: 'priceAsc' },
    { name: 'Price: High-Low', value: 'priceDesc' },
  ];
  shopParams = new ShopParams();
  selectedBrands: string[] = [];
  selectedTypes: string[] = [];
  pageSizeOptions = [5, 10, 15, 20];
  pageEvent?: PageEvent;

  ngOnInit() {
    this.initialiseShop();
  }

  initialiseShop() {
    this.shopService.getTypes();
    this.shopService.getBrands();
    this.getProducts();
  }

  resetFilters() {
    this.shopParams = new ShopParams();
    this.getProducts();
  }

  onSearchChange() {
    this.shopParams.pageNumber = 1;
    this.shopService
      .getProducts(this.selectedBrands, this.selectedTypes, this.shopParams)
      .subscribe({
        next: (response) => (this.products = response),
        error: (error) => console.error(error),
      });
  }

  getProducts() {
    this.shopService
      .getProducts(this.selectedBrands, this.selectedTypes, this.shopParams)
      .subscribe({
        next: (response) => (this.products = response),
        error: (error) => console.error(error),
      });
  }

  onSortChange(event: any) {
    this.shopParams.pageNumber = 1;
    const selectedOption = event.options[0];
    if (selectedOption) {
      this.shopParams.sort = selectedOption.value;
      this.getProducts();
    }
  }

  openFiltersDialog() {
    const dialogRef = this.dialogService.open(FiltersDialogComponent, {
      minWidth: '500px',
      data: {
        selectedBrands: this.shopParams.brands,
        selectedTypes: this.shopParams.types,
      },
    });
    dialogRef.afterClosed().subscribe({
      next: (result) => {
        if (result) {
          this.shopParams.pageNumber = 1;
          this.shopParams.types = result.selectedTypes;
          this.shopParams.brands = result.selectedBrands;
          this.getProducts();
        }
      },
    });
  }

  handlePageEvent(event: PageEvent) {
    this.shopParams.pageNumber = event.pageIndex + 1;
    this.shopParams.pageSize = event.pageSize;
    this.getProducts();
  }
}
