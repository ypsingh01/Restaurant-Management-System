export interface OrderItem {
  orderItemId?: number;
  menuItemId: number;
  menuItemName?: string;
  quantity: number;
  price?: number;
}

export interface Order {
  orderId: number;
  customerId: number;
  orderDate: string;
  status: string;
  totalAmount: number;
  items: OrderItem[];
}

export interface CreateOrderItem {
  menuItemId: number;
  quantity: number;
}

export interface CreateOrder {
  customerId: number;
  items: CreateOrderItem[];
}

