import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './Layout/header/header.component';
import { MatIcon } from '@angular/material/icon';
import { MatButton } from '@angular/material/button';
import { MatBadge } from '@angular/material/badge';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { Product } from '../shared/models/product';
import { Pagination } from '../shared/models/Pagination';

@Component({
  selector: 'app-root',
  imports: [HeaderComponent, CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent implements OnInit {
  baseUrl = 'https://localhost:5001/api/';
  private http = inject(HttpClient);
  protected title = 'client';
  products: Product[] = [];

  ngOnInit(): void {
    this.http.get<Pagination<Product>>(this.baseUrl + 'products').subscribe({
      next: (response) => (this.products = response.data),
      error: (error) => console.log(error),
      complete: () => console.log('Complete'),
    });
  }

  trackByProductId(index: number, product: any): number {
    return product.id;
  }
}
