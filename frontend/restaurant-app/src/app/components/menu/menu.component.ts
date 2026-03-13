import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MenuItem } from '../../models/menu-item';
import { MenuService } from '../../services/menu.service';
import { OrderService } from '../../services/order.service';
import { CreateOrder, CreateOrderItem } from '../../models/order';

@Component({
  selector: 'app-menu',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './menu.component.html',
  styleUrl: './menu.component.css'
})
export class MenuComponent implements OnInit {
  menuItems: MenuItem[] = [];
  cart: { item: MenuItem; quantity: number }[] = [];
  customerId = 1;
  errorMessage = '';
  loading = false;

  constructor(
    private readonly menuService: MenuService,
    private readonly orderService: OrderService
  ) {}

  ngOnInit(): void {
    this.loadMenu();
  }

  loadMenu(): void {
    this.errorMessage = '';
    this.loading = true;
    this.menuService.getMenu().subscribe({
      next: items => {
        this.menuItems = items;
        this.loading = false;
      },
      error: err => {
        this.loading = false;
        this.errorMessage = err?.status === 0
          ? 'Cannot reach the server. Is the backend running on http://localhost:5006?'
          : 'Failed to load menu. Please try again.';
      }
    });
  }

  addToCart(item: MenuItem): void {
    const existing = this.cart.find(c => c.item.menuItemId === item.menuItemId);
    if (existing) {
      existing.quantity++;
    } else {
      this.cart.push({ item, quantity: 1 });
    }
  }

  removeFromCart(index: number): void {
    this.cart.splice(index, 1);
  }

  getTotal(): number {
    return this.cart.reduce((sum, c) => sum + c.item.price * c.quantity, 0);
  }

  placeOrder(): void {
    if (!this.cart.length) {
      this.errorMessage = 'Cart is empty.';
      return;
    }

    const items: CreateOrderItem[] = this.cart.map(c => ({
      menuItemId: c.item.menuItemId,
      quantity: c.quantity
    }));

    const order: CreateOrder = {
      customerId: this.customerId,
      items
    };

    this.orderService.placeOrder(order).subscribe({
      next: () => {
        this.cart = [];
        this.errorMessage = '';
        alert('Order placed successfully');
      },
      error: () => (this.errorMessage = 'Failed to place order.')
    });
  }
}

