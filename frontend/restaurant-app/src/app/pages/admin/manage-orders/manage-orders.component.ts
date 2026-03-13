import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { FormsModule } from '@angular/forms';
import { Order } from '../../../models/order';
import { OrderService } from '../../../services/order.service';
import { ToastService } from '../../../services/toast.service';

@Component({
  selector: 'app-manage-orders',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatTableModule,
    MatSelectModule,
    MatFormFieldModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './manage-orders.component.html',
  styleUrl: './manage-orders.component.css'
})
export class ManageOrdersComponent implements OnInit {
  orders: Order[] = [];
  loading = false;
  statuses = ['Pending', 'Preparing', 'Ready', 'Delivered'];

  constructor(
    private orderService: OrderService,
    private toast: ToastService
  ) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading = true;
    this.orderService.getOrders().subscribe({
      next: (data) => {
        this.orders = data;
        this.loading = false;
      },
      error: () => {
        this.toast.error('Failed to load orders');
        this.loading = false;
      }
    });
  }

  updateStatus(order: Order, status: string): void {
    if (order.status === status) return;
    this.orderService.updateStatus(order.orderId, status).subscribe({
      next: () => {
        order.status = status;
        this.toast.success('Status updated');
      },
      error: () => this.toast.error('Update failed')
    });
  }
}
