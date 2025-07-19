import { Component, Input, input } from '@angular/core';
import { Product } from '../../../../shared/models/product';
import { MatCard } from "@angular/material/card";
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-product-item',
  imports: [MatCard, CommonModule , MatIconModule,MatCardModule],
  templateUrl: './product-item.component.html',
  styleUrl: './product-item.component.scss'
})
export class ProductItemComponent {
  @Input() product?:Product
}
