import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MenuItem } from '../../../models/menu-item';
import { MenuService } from '../../../services/menu.service';
import { ToastService } from '../../../services/toast.service';

@Component({
  selector: 'app-manage-menu',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatCheckboxModule,
    MatDialogModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './manage-menu.component.html',
  styleUrl: './manage-menu.component.css'
})
export class ManageMenuComponent implements OnInit {
  dataSource = new MatTableDataSource<MenuItem>([]);
  displayedColumns = ['name', 'category', 'price', 'isAvailable', 'actions'];
  form: FormGroup;
  editingId: number | null = null;
  loading = false;
  submitting = false;

  constructor(
    private fb: FormBuilder,
    private menuService: MenuService,
    private toast: ToastService
  ) {
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(2)]],
      category: ['', Validators.required],
      price: [0, [Validators.required, Validators.min(0)]],
      isAvailable: [true]
    });
  }

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading = true;
    this.menuService.getMenu().subscribe({
      next: (items) => {
        this.dataSource.data = items;
        this.loading = false;
      },
      error: () => {
        this.toast.error('Failed to load menu');
        this.loading = false;
      }
    });
  }

  edit(item: MenuItem): void {
    this.editingId = item.menuItemId;
    this.form.patchValue({
      name: item.name,
      category: item.category,
      price: item.price,
      isAvailable: item.isAvailable
    });
  }

  cancelEdit(): void {
    this.editingId = null;
    this.form.reset({ name: '', category: '', price: 0, isAvailable: true });
  }

  save(): void {
    if (this.form.invalid || this.submitting) return;
    this.submitting = true;
    const payload = this.form.value;
    if (this.editingId) {
      this.menuService.updateMenuItem(this.editingId, payload).subscribe({
        next: () => {
          this.toast.success('Item updated');
          this.cancelEdit();
          this.load();
          this.submitting = false;
        },
        error: () => {
          this.toast.error('Update failed');
          this.submitting = false;
        }
      });
    } else {
      this.menuService.createMenuItem(payload).subscribe({
        next: () => {
          this.toast.success('Item added');
          this.cancelEdit();
          this.load();
          this.submitting = false;
        },
        error: () => {
          this.toast.error('Add failed');
          this.submitting = false;
        }
      });
    }
  }

  delete(item: MenuItem): void {
    if (!confirm(`Delete "${item.name}"?`)) return;
    this.menuService.deleteMenuItem(item.menuItemId).subscribe({
      next: () => {
        this.toast.success('Item deleted');
        this.load();
      },
      error: () => this.toast.error('Delete failed')
    });
  }
}
