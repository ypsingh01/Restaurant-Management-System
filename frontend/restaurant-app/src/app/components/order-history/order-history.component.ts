import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { Order } from '../../models/order';
import { OrderService } from '../../services/order.service';

@Component({
  selector: 'app-order-history',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './order-history.component.html',
  styleUrl: './order-history.component.css'
})
export class OrderHistoryComponent implements OnInit {
  orders: Order[] = [];
  customerId!: number;
  errorMessage = '';

  constructor(
    private readonly route: ActivatedRoute,
    private readonly orderService: OrderService
  ) {}

  ngOnInit(): void {
    this.customerId = Number(this.route.snapshot.paramMap.get('customerId'));
    this.loadOrders();
  }

  loadOrders(): void {
    this.orderService.getOrdersByCustomer(this.customerId).subscribe({
      next: data => (this.orders = data),
      error: () => (this.errorMessage = 'Failed to load order history.')
    });
  }
}

