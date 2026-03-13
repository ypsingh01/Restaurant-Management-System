import { Injectable, signal, computed } from '@angular/core';
import { MenuItem } from '../models/menu-item';

export interface CartItem {
  item: MenuItem;
  quantity: number;
}

const CART_STORAGE_KEY = 'restaurant_cart';

@Injectable({ providedIn: 'root' })
export class CartService {
  private cartSignal = signal<CartItem[]>(this.loadFromStorage());

  cart = computed(() => this.cartSignal());
  totalItems = computed(() => this.cartSignal().reduce((s, c) => s + c.quantity, 0));
  totalPrice = computed(() =>
    this.cartSignal().reduce((s, c) => s + c.item.price * c.quantity, 0)
  );

  addItem(menuItem: MenuItem, quantity = 1): void {
    const cart = this.cartSignal();
    const existing = cart.find((c) => c.item.menuItemId === menuItem.menuItemId);
    let next: CartItem[];
    if (existing) {
      next = cart.map((c) =>
        c.item.menuItemId === menuItem.menuItemId
          ? { ...c, quantity: c.quantity + quantity }
          : c
      );
    } else {
      next = [...cart, { item: menuItem, quantity }];
    }
    this.cartSignal.set(next);
    this.persist();
  }

  removeItem(menuItemId: number): void {
    this.cartSignal.set(this.cartSignal().filter((c) => c.item.menuItemId !== menuItemId));
    this.persist();
  }

  setQuantity(menuItemId: number, quantity: number): void {
    if (quantity <= 0) {
      this.removeItem(menuItemId);
      return;
    }
    this.cartSignal.set(
      this.cartSignal().map((c) =>
        c.item.menuItemId === menuItemId ? { ...c, quantity } : c
      )
    );
    this.persist();
  }

  clear(): void {
    this.cartSignal.set([]);
    this.persist();
  }

  private loadFromStorage(): CartItem[] {
    try {
      const raw = localStorage.getItem(CART_STORAGE_KEY);
      if (!raw) return [];
      const parsed = JSON.parse(raw) as CartItem[];
      return Array.isArray(parsed) ? parsed : [];
    } catch {
      return [];
    }
  }

  private persist(): void {
    localStorage.setItem(CART_STORAGE_KEY, JSON.stringify(this.cartSignal()));
  }
}
