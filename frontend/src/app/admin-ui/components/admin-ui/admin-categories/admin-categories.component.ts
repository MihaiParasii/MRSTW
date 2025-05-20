import { Component, inject, OnInit } from '@angular/core'
import { CategoryService } from '../../../../services/category/category.service'
import { CategoryResponse } from '../../../../models/category/category-response'
import { RouterLink } from '@angular/router'

@Component({
	selector: 'app-admin-categories',
	imports: [
		RouterLink
	],
	templateUrl: './admin-categories.component.html',
	standalone: true,
	styleUrl: './admin-categories.component.css'
})
export class AdminCategoriesComponent implements OnInit {
	private categoryService: CategoryService = inject(CategoryService)
	protected categories: CategoryResponse[] = []
	
	ngOnInit(): void {
      this.categoryService.get().subscribe(category => {
        this.categories = category
      })
	}
}
