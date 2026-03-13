import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AuthService } from '../../../services/auth.service';
import { OrderService } from '../../../services/order.service';
import { Order } from '../../../models/order';

@Component({
  selector: 'app-order-history',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatTableModule, MatProgressSpinnerModule],
  templateUrl: './order-history.component.html',
  styleUrl: './order-history.component.css'
})
export class CustomerOrderHistoryComponent implements OnInit {
  orders: Order[] = [];
  loading = false;

  constructor(
    private auth: AuthService,
    private orderService: OrderService
  ) {}

  ngOnInit(): void {
    const customerId = this.auth.getCustomerId();
    if (customerId == null) return;
    this.loading = true;
    this.orderService.getOrdersByCustomer(customerId).subscribe({
      next: (data) => {
        this.orders = data;
        this.loading = false;
      },
      error: () => (this.loading = false)
    });
  }
}
