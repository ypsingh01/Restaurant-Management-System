import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Customer } from '../../../models/customer';
import { CustomerService } from '../../../services/customer.service';
import { ToastService } from '../../../services/toast.service';

@Component({
  selector: 'app-admin-customers',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatTableModule, MatProgressSpinnerModule],
  templateUrl: './customers.component.html',
  styleUrl: './customers.component.css'
})
export class AdminCustomersComponent implements OnInit {
  customers: Customer[] = [];
  loading = false;
  displayedColumns = ['customerId', 'name', 'email', 'phone'];

  constructor(
    private customerService: CustomerService,
    private toast: ToastService
  ) {}

  ngOnInit(): void {
    this.loading = true;
    this.customerService.getCustomers().subscribe({
      next: (data) => {
        this.customers = data;
        this.loading = false;
      },
      error: () => {
        this.toast.error('Failed to load customers');
        this.loading = false;
      }
    });
  }
}
