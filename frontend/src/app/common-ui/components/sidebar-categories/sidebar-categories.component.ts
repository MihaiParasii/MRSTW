import { Component, inject, OnInit } from '@angular/core'
import { RouterLink } from '@angular/router'
import { CategoryResponse } from '../../../models/category/category-response'
import { CategoryService } from '../../../services/category/category.service'

@Component({
	selector: 'app-sidebar-categories',
	imports: [
		RouterLink
	],
	templateUrl: './sidebar-categories.component.html',
	standalone: true,
	styleUrl: './sidebar-categories.component.css'
})
export class SidebarCategoriesComponent implements OnInit {
	private categoryService: CategoryService = inject(CategoryService)
	protected categories: CategoryResponse[] = []
	
	ngOnInit(): void {
		this.categoryService.get().subscribe(category => {
			console.log('categories: ', category)
			this.categories = category
		})
	}
}
