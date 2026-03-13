import { inject } from '@angular/core';
import { Router, CanActivateFn, ActivatedRouteSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const roleGuard: CanActivateFn = (route: ActivatedRouteSnapshot) => {
  const auth = inject(AuthService);
  const router = inject(Router);
  const requiredRole = route.data['role'] as string;
  if (!auth.isAuthenticated()) {
    router.navigate(['/']);
    return false;
  }
  if (auth.currentUser()?.role === requiredRole) return true;
  const defaultPath = auth.isAdmin() ? '/admin/dashboard' : '/customer/menu';
  router.navigate([defaultPath]);
  return false;
};
