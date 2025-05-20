import { Component, inject, OnInit } from '@angular/core'
import { SubcategoryResponse } from '../../../models/subcategory/subcategory-response'
import { CategoryResponse } from '../../../models/category/category-response'
import { CategoryService } from '../../../services/category/category.service'
import { SubcategoryService } from '../../../services/subcategory/subcategory.service'
import { FormsModule } from '@angular/forms'
import { CreateDealRequest } from '../../../models/deal/create-deal-request'
import { NgForOf, NgIf } from '@angular/common'
import { RegionService } from '../../../services/region/region.service'
import { RegionResponse } from '../../../models/region/region-response'
import { forkJoin, tap } from 'rxjs'
import { DealService } from '../../../services/deal/deal.service'

@Component({
	selector: 'app-add-product-page',
	imports: [
		FormsModule,
		NgIf,
		NgForOf
	],
	templateUrl: './add-product-page.component.html',
	standalone: true,
	styleUrl: './add-product-page.component.css'
})
export class AddProductPageComponent implements OnInit {
	private categoryService: CategoryService = inject(CategoryService)
	private subcategoryService: SubcategoryService = inject(SubcategoryService)
	private dealService: DealService = inject(DealService)
	private regionService: RegionService = inject(RegionService)
	protected categories: CategoryResponse[] = []
	protected subcategories: SubcategoryResponse[] = []
	protected regions: RegionResponse[] = []
	protected createDealRequest!: CreateDealRequest
	protected photos: File[] = []
	protected photoPreviews: string[] = []
	
	protected allSubcategories: SubcategoryResponse[] = []
	
	ngOnInit(): void {
		this.createDealRequest = {
			title: '',
			description: '',
			categoryId: null,
			subcategoryId: null,
			regionId: null
		}
		
		forkJoin({
			categories: this.categoryService.get(),
			subcategories: this.subcategoryService.get(),
			regions: this.regionService.get()
		}).subscribe(({ categories, subcategories, regions }) => {
			this.categories = categories
			this.allSubcategories = subcategories
			this.regions = regions
		})
	}
	
	onCategoryChange(): void {
		const selectedCategory = this.categories.find(cat => cat.id === this.createDealRequest.categoryId)
		if (selectedCategory) {
			this.subcategories = this.allSubcategories.filter(
					sub => selectedCategory.subcategoryIds.includes(sub.id)
			)
			if (!this.subcategories.some(s => s.id === this.createDealRequest.subcategoryId)) {
				this.createDealRequest.subcategoryId = null
			}
		} else {
			this.subcategories = []
		}
	}

	
	onFileSelect(event: any): void {
		const selectedFiles: File[] = Array.from(event.target.files)
		this.photos = [...this.photos, ...selectedFiles]
		this.generatePreviews()
	}
	
	private generatePreviews(): void {
		this.photoPreviews = []
		for (let photo of this.photos) {
			const reader = new FileReader()
			reader.onload = (e: any) => this.photoPreviews.push(e.target.result)
			reader.readAsDataURL(photo)
		}
	}
	
	onSubmit() {
		console.log('Create Deal Request: Title', this.createDealRequest.title)
		console.log('Create Deal Request: Description', this.createDealRequest.description)
		console.log('Create Deal Request: CategoryId', this.createDealRequest.categoryId)
		console.log('Create Deal Request: SubcategoryId', this.createDealRequest.subcategoryId)
		console.log('Create Deal Request: RegionId', this.createDealRequest.regionId)
		
		this.dealService.create(this.createDealRequest).subscribe()
	}
}
