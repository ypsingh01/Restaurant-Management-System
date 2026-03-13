import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MenuItem } from '../../models/menu-item';
import { MenuService } from '../../services/menu.service';
import { Order } from '../../models/order';
import { OrderService } from '../../services/order.service';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-dashboard.component.html',
  styleUrl: './admin-dashboard.component.css'
})
export class AdminDashboardComponent implements OnInit {
  menuItems: MenuItem[] = [];
  orders: Order[] = [];
  selectedItem: MenuItem | null = null;
  form: Partial<MenuItem> = {
    name: '',
    category: '',
    price: 0,
    isAvailable: true
  };
  errorMessage = '';

  constructor(
    private readonly menuService: MenuService,
    private readonly orderService: OrderService
  ) {}

  ngOnInit(): void {
    this.loadMenu();
    this.loadOrders();
  }

  loadMenu(): void {
    this.menuService.getMenu().subscribe({
      next: data => (this.menuItems = data),
      error: () => (this.errorMessage = 'Failed to load menu.')
    });
  }

  loadOrders(): void {
    this.orderService.getOrders().subscribe({
      next: data => (this.orders = data),
      error: () => (this.errorMessage = 'Failed to load orders.')
    });
  }

  edit(item: MenuItem): void {
    this.selectedItem = item;
    this.form = { ...item };
  }

  resetForm(): void {
    this.selectedItem = null;
    this.form = {
      name: '',
      category: '',
      price: 0,
      isAvailable: true
    };
  }

  save(): void {
    if (!this.form.name || !this.form.category || this.form.price == null) {
      this.errorMessage = 'Name, category, and price are required.';
      return;
    }

    if (this.selectedItem) {
      this.menuService
        .updateMenuItem(this.selectedItem.menuItemId, this.form)
        .subscribe({
          next: () => {
            this.loadMenu();
            this.resetForm();
          },
          error: () => (this.errorMessage = 'Failed to update menu item.')
        });
    } else {
      this.menuService.createMenuItem(this.form).subscribe({
        next: () => {
          this.loadMenu();
          this.resetForm();
        },
        error: () => (this.errorMessage = 'Failed to create menu item.')
      });
    }
  }

  delete(item: MenuItem): void {
    if (!confirm(`Delete menu item "${item.name}"?`)) return;

    this.menuService.deleteMenuItem(item.menuItemId).subscribe({
      next: () => this.loadMenu(),
      error: () => (this.errorMessage = 'Failed to delete menu item.')
    });
  }

  updateOrderStatus(order: Order, status: string): void {
    this.orderService.updateStatus(order.orderId, status).subscribe({
      next: () => this.loadOrders(),
      error: () => (this.errorMessage = 'Failed to update order status.')
    });
  }
}

