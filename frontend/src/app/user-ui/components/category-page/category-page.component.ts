import { Component, inject, OnInit } from '@angular/core'
import { SubcategoryService } from '../../../services/subcategory/subcategory.service'
import { SubcategoryResponse } from '../../../models/subcategory/subcategory-response'
import { ActivatedRoute, RouterLink } from '@angular/router'
import { CategoryService } from '../../../services/category/category.service'
import { tap } from 'rxjs'
import { NgForOf } from '@angular/common'

@Component({
	selector: 'app-category-page',
	standalone: true,
	imports: [RouterLink, NgForOf],
	templateUrl: './category-page.component.html',
	styleUrl: './category-page.component.css'
})
export class CategoryPageComponent implements OnInit {
	private categoryService = inject(CategoryService)
	private subcategoryService = inject(SubcategoryService)
	private route = inject(ActivatedRoute)
	
	protected subcategories: SubcategoryResponse[] = []
	protected id = 0
	
	ngOnInit(): void {
		this.route.paramMap.subscribe(params => {
			const id = Number(params.get('categoryId'))
			if (!id) return
			
			this.id = id
			this.subcategories = [] // Clear previous subcategories
			
			this.categoryService.getById(id).subscribe(category => {
				console.log('category', category)
				
				category.subcategoryIds.forEach(subcatId => {
					this.subcategoryService.getById(subcatId)
							.pipe(tap(sub => this.subcategories.push(sub)))
							.subscribe()
				})
			})
		})
	}
	
	trackById(index: number, item: SubcategoryResponse) {
		return item.id;
	}
	
}
