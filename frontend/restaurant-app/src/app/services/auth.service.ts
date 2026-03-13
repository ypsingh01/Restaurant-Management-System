import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap, catchError, of } from 'rxjs';
import { AuthResponse, LoginRequest, RegisterRequest } from '../models/user';
import { environment } from '../../environments/environment';

const TOKEN_KEY = 'restaurant_token';
const USER_KEY = 'restaurant_user';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly api = environment.apiBaseUrl + 'auth';
  private tokenSignal = signal<string | null>(this.getStoredToken());
  private userSignal = signal<AuthResponse | null>(this.getStoredUser());

  currentUser = computed(() => this.userSignal());
  isAuthenticated = computed(() => !!this.tokenSignal());
  token = computed(() => this.tokenSignal());

  constructor(
    private http: HttpClient,
    private router: Router
  ) {}

  login(body: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.api}/login`, body).pipe(
      tap((res) => this.setSession(res))
    );
  }

  register(body: RegisterRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.api}/register`, body).pipe(
      tap((res) => this.setSession(res))
    );
  }

  logout(): void {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(USER_KEY);
    this.tokenSignal.set(null);
    this.userSignal.set(null);
    this.router.navigate(['/']);
  }

  getToken(): string | null {
    return this.tokenSignal();
  }

  isAdmin(): boolean {
    return this.userSignal()?.role === 'Admin';
  }

  isCustomer(): boolean {
    return this.userSignal()?.role === 'Customer';
  }

  getCustomerId(): number | null {
    return this.userSignal()?.customerId ?? null;
  }

  private setSession(res: AuthResponse): void {
    localStorage.setItem(TOKEN_KEY, res.token);
    localStorage.setItem(USER_KEY, JSON.stringify(res));
    this.tokenSignal.set(res.token);
    this.userSignal.set(res);
  }

  private getStoredToken(): string | null {
    return localStorage.getItem(TOKEN_KEY);
  }

  private getStoredUser(): AuthResponse | null {
    try {
      const raw = localStorage.getItem(USER_KEY);
      return raw ? JSON.parse(raw) : null;
    } catch {
      return null;
    }
  }
}
