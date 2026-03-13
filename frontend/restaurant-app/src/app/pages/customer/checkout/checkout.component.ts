import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AuthService } from '../../../services/auth.service';
import { CartService } from '../../../services/cart.service';
import { OrderService } from '../../../services/order.service';
import { ToastService } from '../../../services/toast.service';

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatButtonModule, MatProgressSpinnerModule],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.css'
})
export class CheckoutComponent {
  submitting = false;

  constructor(
    private auth: AuthService,
    public cart: CartService,
    private orderService: OrderService,
    private toast: ToastService,
    private router: Router
  ) {}

  placeOrder(): void {
    const customerId = this.auth.getCustomerId();
    if (customerId == null) {
      this.toast.error('You must be logged in as a customer.');
      return;
    }
    const items = this.cart.cart();
    if (!items.length) {
      this.toast.error('Cart is empty.');
      return;
    }
    this.submitting = true;
    this.orderService
      .placeOrder({
        customerId,
        items: items.map((c) => ({ menuItemId: c.item.menuItemId, quantity: c.quantity }))
      })
      .subscribe({
        next: () => {
          this.cart.clear();
          this.toast.success('Order placed successfully!');
          this.router.navigate(['/customer/orders']);
          this.submitting = false;
        },
        error: (err) => {
          this.toast.error(err.error?.message || 'Failed to place order.');
          this.submitting = false;
        }
      });
  }
}
