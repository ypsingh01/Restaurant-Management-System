export interface Customer {
  customerId: number;
  name: string;
  email?: string;
  phone?: string;
}

export interface CreateCustomer {
  name: string;
  email?: string;
  phone?: string;
}

