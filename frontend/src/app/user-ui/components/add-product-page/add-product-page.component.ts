import {Component, inject} from '@angular/core';
import {SubcategoryResponse} from '../../../models/subcategory/subcategory-response';
import {CategoryResponse} from '../../../models/category/category-response';
import {CategoryService} from '../../../services/category/category.service';
import {SubcategoryService} from '../../../services/subcategory/subcategory.service';
import {FormsModule} from '@angular/forms';
import {CreateDealRequest} from '../../../models/deal/create-deal-request';
import {NgForOf, NgIf} from '@angular/common';
import {RegionService} from '../../../services/region/region.service';
import {RegionResponse} from '../../../models/region/region-response';

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
export class AddProductPageComponent {
  private categoryService: CategoryService = inject(CategoryService);
  private subcategoryService: SubcategoryService = inject(SubcategoryService);
  private regionService: RegionService = inject(RegionService);
  protected categories: CategoryResponse[] = [];
  protected subcategories: SubcategoryResponse[] = [];
  protected regions: RegionResponse[] = [];
  protected createDealRequest: CreateDealRequest = new CreateDealRequest();
  protected photos: File[] = [];
  protected photoPreviews: string[] = [];


  constructor() {
    this.regionService.get().subscribe(value => {
      this.regions = value;
    })

    this.categoryService.get().subscribe(value => {
      this.categories = value;
    })

    this.subcategoryService.get().subscribe(value => {
      this.subcategories = value;
    })

  }

  onFileSelect(event: any): void {
    const selectedFiles: File[] = Array.from(event.target.files);
    this.photos = [...this.photos, ...selectedFiles];
    this.generatePreviews();
  }

  private generatePreviews(): void {
    this.photoPreviews = [];
    for (let photo of this.photos) {
      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.photoPreviews.push(e.target.result);
      };
      reader.readAsDataURL(photo);
    }
  }


  onCategoryChange() {
    this.subcategoryService.get().subscribe(value => {
      this.subcategories = value;
    });
  }

  onSubmit() {
    console.log('Create Deal Request: Title', this.createDealRequest.Title);
    console.log('Create Deal Request: Description', this.createDealRequest.Description);
    console.log('Create Deal Request: CategoryId', this.createDealRequest.CategoryId);
    console.log('Create Deal Request: SubcategoryId', this.createDealRequest.SubcategoryId);
    console.log('Create Deal Request: RegionId', this.createDealRequest.RegionId);

    this.photos.forEach(photo => {
      console.log(`Create Deal Request: Photo ${photo.name}`);
    })
  }
}
