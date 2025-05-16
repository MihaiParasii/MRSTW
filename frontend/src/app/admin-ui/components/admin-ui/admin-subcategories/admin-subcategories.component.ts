import { Component, inject, OnInit } from '@angular/core'
import { CategoryService } from '../../../../services/category/category.service'
import { getIdFromRoute } from '../../../../common/get-id-from-route'
import { ActivatedRoute } from '@angular/router'
import { CategoryResponse } from '../../../../models/category/category-response'
import { tap } from 'rxjs'
import { SubcategoryResponse } from '../../../../models/subcategory/subcategory-response'
import { SubcategoryService } from '../../../../services/subcategory/subcategory.service'

@Component({
	selector: 'app-admin-subcategories',
	imports: [],
	templateUrl: './admin-subcategories.component.html',
	standalone: true,
	styleUrl: './admin-subcategories.component.css'
})
export class AdminSubcategoriesComponent implements OnInit {
	private categoryService: CategoryService = inject(CategoryService)
	private subcategoryService: SubcategoryService = inject(SubcategoryService)
	private route: ActivatedRoute = inject(ActivatedRoute)
	private category!: CategoryResponse
	protected subcategories: SubcategoryResponse[] = []
	
	
	ngOnInit(): void {
		const id = getIdFromRoute(this.route)
		
		if (id) {
			
			const category$ = this.categoryService.getById(id).pipe(tap((category) => {
				this.category = category
			}))
			
			category$.pipe(tap(() => {
				this.category.subcategoryIds.forEach(id => {
					this.subcategoryService.getById(id).pipe(tap((subcategory) => {
						this.subcategories.push(subcategory)
					}))
				})
			}))
		}
	}
}
