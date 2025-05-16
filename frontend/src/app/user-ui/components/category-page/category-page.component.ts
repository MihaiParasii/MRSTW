import {Component, inject} from '@angular/core';
import {SubcategoryService} from '../../../services/subcategory/subcategory.service';
import {SubcategoryResponse} from '../../../models/subcategory/subcategory-response';
import {ActivatedRoute, RouterLink} from '@angular/router';

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
  protected categoryUrlId!: string;

  constructor(private route: ActivatedRoute) {
    this.subcategoryService.get().subscribe(value => {
      this.subcategories = value;
    })

    this.categoryUrlId = <string>route.snapshot.paramMap.get('category-id');
  }
}
