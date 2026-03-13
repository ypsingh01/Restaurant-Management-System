import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MenuItem } from '../models/menu-item';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MenuService {
  private readonly baseUrl = environment.apiBaseUrl + 'menu';

  constructor(private http: HttpClient) {}

  getMenu(): Observable<MenuItem[]> {
    return this.http.get<MenuItem[]>(this.baseUrl);
  }

  getMenuItem(id: number): Observable<MenuItem> {
    return this.http.get<MenuItem>(`${this.baseUrl}/${id}`);
  }

  createMenuItem(item: Partial<MenuItem>): Observable<MenuItem> {
    return this.http.post<MenuItem>(this.baseUrl, item);
  }

  updateMenuItem(id: number, item: Partial<MenuItem>): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, item);
  }

  deleteMenuItem(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}

