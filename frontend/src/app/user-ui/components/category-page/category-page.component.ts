import {Component, inject} from '@angular/core';
import {SubcategoryService} from '../../../services/subcategory/subcategory.service';
import {SubcategoryResponse} from '../../../models/subcategory/subcategory-response';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-category-page',
  imports: [
    RouterLink
  ],
  templateUrl: './category-page.component.html',
  standalone: true,
  styleUrl: './category-page.component.css'
})
export class CategoryPageComponent {

  private subcategoryService: SubcategoryService = inject(SubcategoryService);
  protected subcategories: SubcategoryResponse[] = [];

  constructor() {
    this.subcategoryService.get().subscribe(value => {
      this.subcategories = value;
    })
  }
}
