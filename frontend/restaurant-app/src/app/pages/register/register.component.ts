import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AuthService } from '../../services/auth.service';
import { ToastService } from '../../services/toast.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  form: FormGroup;
  loading = false;
  role: string;

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private router: Router,
    private route: ActivatedRoute,
    private toast: ToastService
  ) {
    this.role = this.route.snapshot.paramMap.get('role') ?? 'customer';
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      phone: ['']
    });
  }

  onSubmit(): void {
    if (this.form.invalid || this.loading) return;
    const role = this.role === 'admin' ? 'Admin' : 'Customer';
    this.loading = true;
    this.auth
      .register({
        ...this.form.value,
        role,
        phone: this.form.get('phone')?.value || undefined
      })
      .subscribe({
        next: (user) => {
          this.loading = false;
          this.toast.success('Account created!');
          if (user.role === 'Admin') this.router.navigate(['/admin/dashboard']);
          else this.router.navigate(['/customer/menu']);
        },
        error: (err) => {
          this.loading = false;
          this.toast.error(err.error?.message || 'Registration failed.');
        }
      });
  }
}
