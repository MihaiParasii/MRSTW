import {Component, inject} from '@angular/core';
import {RouterLink} from '@angular/router';
import {CategoryResponse} from '../../../models/category/category-response';
import {CategoryService} from '../../../services/category/category.service';

@Component({
  selector: 'app-sidebar-categories',
  imports: [
    RouterLink
  ],
  templateUrl: './sidebar-categories.component.html',
  standalone: true,
  styleUrl: './sidebar-categories.component.css'
})
export class SidebarCategoriesComponent {
  private categoryService: CategoryService = inject(CategoryService);
  protected categories: CategoryResponse[] = [];

  constructor() {
    this.categoryService.get().subscribe(value => {
      this.categories = value;
    })
  }
}
