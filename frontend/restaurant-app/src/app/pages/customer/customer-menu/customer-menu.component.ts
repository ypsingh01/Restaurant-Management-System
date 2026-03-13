import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MenuItem } from '../../../models/menu-item';
import { MenuService } from '../../../services/menu.service';
import { CartService } from '../../../services/cart.service';
import { ToastService } from '../../../services/toast.service';

@Component({
  selector: 'app-customer-menu',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './customer-menu.component.html',
  styleUrl: './customer-menu.component.css'
})
export class CustomerMenuComponent implements OnInit {
  items: MenuItem[] = [];
  loading = false;
  error = '';

  constructor(
    private menuService: MenuService,
    private cart: CartService,
    private toast: ToastService
  ) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading = true;
    this.error = '';
    this.menuService.getMenu().subscribe({
      next: (data) => {
        this.items = data;
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to load menu.';
        this.loading = false;
      }
    });
  }

  addToCart(item: MenuItem): void {
    if (!item.isAvailable) return;
    this.cart.addItem(item, 1);
    this.toast.success(`Added ${item.name} to cart`);
  }
}
