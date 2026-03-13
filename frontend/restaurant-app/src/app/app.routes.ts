import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';
import { roleGuard } from './guards/role.guard';

export const routes: Routes = [
  { path: '', loadComponent: () => import('./pages/role-selection/role-selection.component').then(m => m.RoleSelectionComponent) },
  { path: 'login/:role', loadComponent: () => import('./pages/login/login.component').then(m => m.LoginComponent) },
  { path: 'register/:role', loadComponent: () => import('./pages/register/register.component').then(m => m.RegisterComponent) },

  {
    path: 'admin',
    loadComponent: () => import('./layouts/admin-layout/admin-layout.component').then(m => m.AdminLayoutComponent),
    canActivate: [authGuard, roleGuard],
    data: { role: 'Admin' },
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', loadComponent: () => import('./pages/admin/admin-dashboard/admin-dashboard.component').then(m => m.AdminDashboardComponent) },
      { path: 'menu', loadComponent: () => import('./pages/admin/manage-menu/manage-menu.component').then(m => m.ManageMenuComponent) },
      { path: 'orders', loadComponent: () => import('./pages/admin/manage-orders/manage-orders.component').then(m => m.ManageOrdersComponent) },
      { path: 'customers', loadComponent: () => import('./pages/admin/customers/customers.component').then(m => m.AdminCustomersComponent) }
    ]
  },

  {
    path: 'customer',
    loadComponent: () => import('./layouts/customer-layout/customer-layout.component').then(m => m.CustomerLayoutComponent),
    canActivate: [authGuard, roleGuard],
    data: { role: 'Customer' },
    children: [
      { path: '', redirectTo: 'menu', pathMatch: 'full' },
      { path: 'menu', loadComponent: () => import('./pages/customer/customer-menu/customer-menu.component').then(m => m.CustomerMenuComponent) },
      { path: 'cart', loadComponent: () => import('./pages/customer/cart/cart.component').then(m => m.CartComponent) },
      { path: 'checkout', loadComponent: () => import('./pages/customer/checkout/checkout.component').then(m => m.CheckoutComponent) },
      { path: 'orders', loadComponent: () => import('./pages/customer/order-history/order-history.component').then(m => m.CustomerOrderHistoryComponent) }
    ]
  },

  { path: '**', redirectTo: '' }
];
