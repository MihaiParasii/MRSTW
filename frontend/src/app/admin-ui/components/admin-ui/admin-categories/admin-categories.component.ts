import {Component, inject} from '@angular/core';
import {CategoryService} from '../../../../services/category/category.service';
import {CategoryResponse} from '../../../../models/category/category-response';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-admin-categories',
  imports: [
    RouterLink
  ],
  templateUrl: './admin-categories.component.html',
  standalone: true,
  styleUrl: './admin-categories.component.css'
})
export class AdminCategoriesComponent {
  private categoryService: CategoryService = inject(CategoryService);
  protected categories: CategoryResponse[] = [];

  constructor() {
    this.categoryService.get().subscribe(category => {
      this.categories = category;
    })
  }

}
