import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { forkJoin } from 'rxjs';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MenuService } from '../../../services/menu.service';
import { OrderService } from '../../../services/order.service';
import { CustomerService } from '../../../services/customer.service';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './admin-dashboard.component.html',
  styleUrl: './admin-dashboard.component.css'
})
export class AdminDashboardComponent implements OnInit {
  menuCount = 0;
  orderCount = 0;
  customerCount = 0;
  totalSales = 0;
  loading = true;
  error = '';

  constructor(
    private menuService: MenuService,
    private orderService: OrderService,
    private customerService: CustomerService
  ) {}

  ngOnInit(): void {
    forkJoin({
      menu: this.menuService.getMenu(),
      orders: this.orderService.getOrders(),
      customers: this.customerService.getCustomers()
    }).subscribe({
      next: ({ menu, orders, customers }) => {
        this.menuCount = menu.length;
        this.orderCount = orders.length;
        this.customerCount = customers.length;
        this.totalSales = orders.reduce((s, o) => s + o.totalAmount, 0);
      },
      error: () => (this.error = 'Failed to load summary'),
      complete: () => (this.loading = false)
    });
  }
}
